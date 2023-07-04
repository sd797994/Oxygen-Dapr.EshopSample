using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Redis.StackExchange;
using JobService;
using JobService.Modules;
using Oxygen.IocModule;
using Oxygen.Server.Kestrel.Implements;

IConfiguration Configuration = default;
var builder = OxygenApplication.CreateBuilder(config =>
{
    config.Port = 80;
    config.PubSubCompentName = "pubsub";
    config.StateStoreCompentName = "statestore";
    config.TracingHeaders = "Authentication,AuthIgnore";
});
OxygenStartup.ConfigureServices(builder.Services);
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
});
builder.Services.AddLogging(configure =>
{
    configure.AddConfiguration(Configuration.GetSection("Logging"));
    configure.AddConsole();
});
GlobalConfiguration.Configuration.UseRedisStorage(Configuration.GetSection("RedisConnection").Value);
builder.Services.AddHangfire(x => { });
builder.Services.AddHangfireServer();
builder.Services.AddHostedService<CronJobService>();//注册并运行所有的周期作业
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
var app = builder.Build();
OxygenStartup.Configure(app, app.Services);
await app.RunAsync();