using IApplicationService.Base.SentinelConfig;
using IApplicationService.PublicService.Dtos.Input;
using Infrastructure.Common.AliSentinel.Extension;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 针对阿里sentinel组件的dapr支持
    /// </summary>
    public class AliSentinelConfigManager
    {
        static Kubernetes kubernetes = new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile(SentinelComponentBaseConfig.kubeconfig));
        /// <summary>
        /// 注册规则
        /// </summary>
        /// <param name="aliSentinelConfig"></param>
        public static async Task RegisterSentinelConfig(SentinelConfigList aliSentinelConfigList)
        {
            await GetAndSaveSentinelComponent(component =>
            {
                component.FlowRules = aliSentinelConfigList.FlowRules.GetDistinct();
                component.BreakingRules = aliSentinelConfigList.BreakingRules.GetDistinct();
            });
        }
        /// <summary>
        /// 获取所有注册规则
        /// </summary>
        /// <returns></returns>
        public static async Task<SentinelConfigList> GetAll()
        {
            var component = await GetDefaultSentinelComponent();
            return new SentinelConfigList()
            {
                FlowRules = component.FlowRules,
                BreakingRules = component.BreakingRules
            };
        }
        #region 本地方法
        /// <summary>
        /// 获取默认的SentinelComponent
        /// </summary>
        /// <returns></returns>
        static async Task<SentinelComponent> GetDefaultSentinelComponent()
        {
            var component = new SentinelComponent();
            await component.Create(kubernetes);
            return component;
        }
        /// <summary>
        /// 传递委托变更默认SentinelComponent
        /// </summary>
        /// <param name="operatorComponent"></param>
        static async Task GetAndSaveSentinelComponent(Action<SentinelComponent> operatorComponent)
        {
            var component = await GetDefaultSentinelComponent();
            operatorComponent(component);
            component.SetMetaData();
            Patch(component);
            ReloadDeploy();
        }
        /// <summary>
        /// Patch SentinelComponent到k8s环境
        /// </summary>
        /// <param name="component"></param>
        static void Patch(SentinelComponent component)
        {
            var patch = new JsonPatchDocument<SentinelComponent>();
            patch.Replace(x => x.spec.metadata, component.spec.metadata);
            kubernetes.PatchNamespacedCustomObject(new V1Patch(patch, V1Patch.PatchType.JsonPatch), SentinelComponentBaseConfig.Group, SentinelComponentBaseConfig.Version, SentinelComponentBaseConfig.NamespaceParameter, SentinelComponentBaseConfig.Plural, SentinelComponentBaseConfig.ComponentName);
        }
        /// <summary>
        /// 重启相关deploy更新SentinelComponent
        /// </summary>
        static void ReloadDeploy()
        {
            var deploy = kubernetes.ReadNamespacedDeployment(SentinelComponentBaseConfig.DeploymentName, SentinelComponentBaseConfig.NamespaceParameter);
            deploy.Spec.Template.Metadata.Annotations[SentinelComponentBaseConfig.restart] = DateTime.UtcNow.ToString("s");
            var patch = new JsonPatchDocument<V1Deployment>();
            patch.Replace(e => e.Spec.Template.Metadata.Annotations, deploy.Spec.Template.Metadata.Annotations);
            kubernetes.PatchNamespacedDeployment(new V1Patch(patch, V1Patch.PatchType.JsonPatch), SentinelComponentBaseConfig.DeploymentName, SentinelComponentBaseConfig.NamespaceParameter);
        }
        #endregion
    }
}
