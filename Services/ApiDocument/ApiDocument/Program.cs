using ApiDocument;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDocument", Version = "v1" });
    c.OperationFilter<AddAuthTokenHeaderParameter>();
    c.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "ApiDocument.xml"));
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDocument v1");
});
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
await app.RunAsync();