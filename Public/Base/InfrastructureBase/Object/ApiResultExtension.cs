using IApplicationService;
using InfrastructureBase.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DomainBase;

namespace InfrastructureBase.Object
{

    public static class ApiResultExtension
    {
        public static async Task<ApiResult> RunActorAsync(this ApiResult apiResult, Func<Task> actorInvoke)
        {
            try
            {
                await actorInvoke();
                return apiResult;
            }
            catch (Exception e)
            {
                if (e is ApplicationServiceException || e is DomainException || e is InfrastructureException)
                {
                    return ApiResult.Err(e.Message);
                }
                return ApiResult.Err();
            }
        }
        public static async Task<ApiResult> Async(this ApiResult apiResult)
        {
            return await Task.FromResult(apiResult);
        }
        public static async Task<ApiResult> Async<T>(this ApiResult<T> apiResult)
        {
            apiResult.Data = await apiResult.TaskData;
            return apiResult;
        }
        public static T GetData<T>(this ApiResult apiResult)
        {
            if (apiResult == null)
                return default;
            if (apiResult.Data == null)
                return default;
            var json = JsonSerializer.Serialize(apiResult.Data);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
