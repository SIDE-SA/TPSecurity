using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TPSecurity.Api.Http;

namespace TPSecurity.Api.Filter
{
    public class UserHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = HttpContextItemKeys.User,
                In = ParameterLocation.Header,
                Required = true,
                AllowEmptyValue = false,
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
