using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// 基础配置文件用于访问k8s集群中的特定CRD资源
    /// </summary>
    public class SentinelComponentBaseConfig
    {
        public const string Group = "dapr.io";
        public const string Version = "v1alpha1";
        public const string NamespaceParameter = "dapreshop";
        public const string Plural = "components";
        public const string ComponentName = "sentinel";
        public const string DeploymentName = "apigateway";
        public const string kubeconfig = "/.kube/config";
        public const string flowrulename = "flowRules";
        public const string circuitbreakerrulename = "circuitbreakerRules";
        public const string restart = "kubectl.kubernetes.io/restartedAt";
    }
}
