using Autofac;
using Autofac.Extensions.DependencyInjection;
using ImageService.ImageAppService;
using ImageService.Modules;
using Infrastructure.Http;
using Oxygen.IocModule;
using Oxygen.Mesh.Dapr;
using Oxygen.ProxyGenerator.Implements;
using Oxygen.Server.Kestrel.Implements;

IConfiguration Configuration = default;
var builder = OxygenApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseStaticFiles = true;
    config.UseCors = true;
});
OxygenActorStartup.ConfigureServices(builder.Services);
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory());
    config.AddJsonFile("appsettings.json");
    Configuration = config.Build();
}).ConfigureContainer<ContainerBuilder>(builder =>
{
    //注入oxygen依赖
    builder.RegisterOxygenModule();
    //注入业务依赖
    builder.RegisterModule(new ServiceModule());
    builder.RegisterType<ImageAppService>().As<IApplicationService.Base.IImageAppService>().InstancePerLifetimeScope();
});
//注册全局拦截器
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
//注册鉴权拦截器
ImageAuthenticationHandler.RegisterAllFilter();
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
OxygenActorStartup.Configure(app, app.Services);
await app.RunAsync();