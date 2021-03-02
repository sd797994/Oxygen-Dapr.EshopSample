using Domain.Entities;
using Domain.Repository;
using IApplicationService;
using IApplicationService.PublicService;
using Infrastructure.EfDataAccess;
using InfrastructureBase.AuthBase;
using InfrastructureBase.Object;
using Oxygen.Client.ServerProxyFactory.Interface;
using System.Threading.Tasks;
using IApplicationService.Base.AppQuery;
using System.Linq;
using InfrastructureBase.Data;
using Microsoft.EntityFrameworkCore;
using IApplicationService.PublicService.Dtos.Output;

namespace ApplicationService
{
    public class MallSettingQueryService : IMallSettingQueryService
    {
        private readonly EfDbContext dbContext;
        private readonly IStateManager stateManager;
        public MallSettingQueryService(EfDbContext dbContext, IStateManager stateManager)
        {
            this.dbContext = dbContext;
            this.stateManager = stateManager;
        }

        [AuthenticationFilter]
        public async Task<ApiResult> GetMallSetting()
        {
            var mallsetting = await dbContext.MallSetting.FirstOrDefaultAsync();
            if (mallsetting == null)
                return ApiResult.Ok(new MallSettingOutInfo("",""));
            else
                return ApiResult.Ok(new MallSettingOutInfo(mallsetting.DeliverName, mallsetting.DeliverAddress));
        }
    }
}
