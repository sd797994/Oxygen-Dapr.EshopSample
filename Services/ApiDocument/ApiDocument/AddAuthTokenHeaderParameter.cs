using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDocument
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "Authentication",
                In = ParameterLocation.Header,
                Required = false
            });
        }
    }
}
