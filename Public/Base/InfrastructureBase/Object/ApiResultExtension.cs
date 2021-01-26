using IApplicationService;
using InfrastructureBase.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace InfrastructureBase.Object
{

    public static class ApiResultExtension
    {
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
