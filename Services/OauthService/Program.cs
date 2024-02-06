using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Http;
using OauthService.Modules;
using Oxygen.IocModule;
using Oxygen.ProxyGenerator.Implements;
using Oxygen.Server.Kestrel.Implements;


IConfiguration Configuration = default;
var builder = OxygenApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
    config.UseCors = true;
});
OxygenStartup.ConfigureServices(builder.Services);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json");
Configuration = builder.Configuration;
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    //×¢ÈëoxygenÒÀÀµ
    builder.RegisterOxygenModule();
    //×¢ÈëÒµÎñÒÀÀµ
    builder.RegisterModule(new ServiceModule());
});
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddHttpClient();
//×¢²áÈ«¾ÖÀ¹½ØÆ÷
LocalMethodAopProvider.RegisterPipelineHandler(AopHandlerProvider.ContextHandler, AopHandlerProvider.BeforeSendHandler, AopHandlerProvider.AfterMethodInvkeHandler, AopHandlerProvider.ExceptionHandler);
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
OxygenStartup.Configure(app, app.Services);
await app.RunAsync();