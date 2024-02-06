using Autofac;
using Autofac.Extensions.DependencyInjection;
using Host;
using Host.Modules;
using IApplicationService.AppEvent;
using IApplicationService.PublicService.Dtos.Event;
using Infrastructure.EfDataAccess;
using Infrastructure.Http;
using InfrastructureBase.AopFilter;
using InfrastructureBase.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Common.Interface;
using Oxygen.Common;
using Oxygen.IocModule;
using Oxygen.Mesh.Dapr;
using Oxygen.ProxyGenerator.Implements;
using Oxygen.Server.Kestrel.Implements;
using Saga;
using Saga.PubSub.Dapr;
using Saga.Store.Dapr;
using System.Text.Json;

IConfiguration Configuration = default;
var builder = OxygenApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseCors = true;
});
OxygenActorStartup.ConfigureServices(builder.Services);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json");
Configuration = builder.Configuration;
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    //注入oxygen依赖
    builder.RegisterOxygenModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddHttpClient();
//注册自定义HostService
builder.Services.AddHostedService<CustomerService>();
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
GoodsAuthenticationHandler.RegisterAllFilter();
//注册自定义Attribute AOP拦截器(需要注册全局拦截器才有效)
AopFilterManager.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddDbContext<EfDbContext>(options => options.UseNpgsql(Configuration.GetSection("SqlConnectionString").Value));
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddSaga(new SagaConfiguration("Oxygen-Dapr.EshopSample", "GoodsService", null, null, new IApplicationService.Sagas.CreateOrder.Configuration()));
builder.Services.AddSagaStore();
var app = builder.Build();
app.RegisterSagaHandler(async (service, error) =>
{
    //当出现补偿异常的saga流时，会触发这个异常处理器，需要人工进行处理(持久化消息/告警通知等等)
    //此处作为演示，我将会直接导入到事件异常服务
    await service.GetService<IEventBus>().SendEvent(EventTopicDictionary.Common.EventHandleErrCatch,
                   new EventHandlerErrDto($"Saga流[{error.SourceTopic}]事件补偿异常", error.SourceDataJson, error.SourceException.Message, error.SourceException.StackTrace, false));
});
OxygenActorStartup.Configure(app, app.Services);
await app.RunAsync();