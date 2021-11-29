using DomainBase;
using IApplicationService;
using InfrastructureBase;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Oxygen.Common.Implements;

namespace Infrastructure.Http
{
    public class AopHandlerProvider
    {
        public static void ContextHandler(OxygenHttpContextWapper oxygenHttpContext)
        {
            HttpContextExt.SetCurrent(oxygenHttpContext);//注入http上下文给本地业务上下文对象
        }
        public static async Task BeforeSendHandler(object param, OxygenHttpContextWapper oxygenHttpContext)
        {
            await Task.CompletedTask;
        }
        public static async Task AfterMethodInvkeHandler(object result)
        {
            await Task.CompletedTask;
        }

        public static async Task<object> ExceptionHandler(Exception exception)
        {
            //异常处理
            if (exception is ApplicationServiceException || exception is DomainException || exception is InfrastructureException)
            {
                return await ApiResult.Err(exception.Message).Async();
            }
            else
            {
                Console.WriteLine("系统异常：" + exception.Message);
                return await ApiResult.Err().Async();
            }
        }
    }
}
