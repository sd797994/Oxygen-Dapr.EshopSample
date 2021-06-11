using IApplicationService;
using IApplicationService.Base;
using IApplicationService.Base.SentinelConfig;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureBase.Object;
using IApplicationService.PublicService.Dtos.Input;
using InfrastructureBase;
using IApplicationService.Base.SentinelConfig.Enums;

namespace ApplicationService
{
    public class AliSentinelConfigService : IAliSentinelConfigService
    {
        /// <summary>
        /// 获取配置节
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult> Get()
        {
            var result = await AliSentinelConfigManager.GetAll();
            return ApiResult.Ok(result);
        }
        /// <summary>
        /// 保存配置节
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<ApiResult> Save(SentinelConfigList config)
        {
            await AliSentinelConfigManager.RegisterSentinelConfig(config);
            return ApiResult.Ok();
        }
        /// <summary>
        /// 获取配置节相关基础数据
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult> GetCommonData()
        {
            var services = Common.GetRemoteServicesInfo();
            var controlBehavior = Common.GetEnumValue<ControlBehavior>().ToList();
            var strategy = Common.GetEnumValue<Strategy>().ToList();
            return await ApiResult.Ok(new { services, controlBehavior, strategy }).Async();
        }
    }
}
