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
        public static async Task<ApiResult> RunAsync(this ApiResult apiResult, Func<Task> invokeAsync, Func<Task> catchAsync = null)
        {
            try
            {
                await invokeAsync();
                return apiResult;
            }
            catch (Exception e)
            {
                try
                {
                    if (catchAsync != null)
                        await catchAsync();
                }
                finally { }
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
            try
            {
                apiResult.Data = await apiResult.TaskData;
            }
            catch (Exception) { return ApiResult.Err(); }
            return apiResult;
        }
        public static T GetData<T>(this ApiResult apiResult)
        {
            if (apiResult == null)
                return default;
            if (apiResult.Data == null)
                return default;
            if(apiResult.Code!=0 && !string.IsNullOrEmpty(apiResult.Message))
            {
                throw new ApplicationServiceException(apiResult.Message);
            }
            var json = JsonSerializer.Serialize(apiResult.Data, JsonSerializerDefaultOption.Default);
            return JsonSerializer.Deserialize<T>(json, JsonSerializerDefaultOption.Default);
        }
    }
}
