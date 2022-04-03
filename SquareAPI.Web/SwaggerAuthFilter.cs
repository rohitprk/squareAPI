using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SquareAPI.Web
{
    /// <summary>
    /// Class to add custom Swagger Operation Filter.
    /// </summary>
    public class SwaggerAuthFilter : IOperationFilter
    {
        /// <summary>
        /// Method to apply filter on swagger doc to generate Security for Authorize Method only.
        /// </summary>
        /// <param name="operation">OpenApiOperation object to set security.</param>
        /// <param name="ctx">Operational filter context to retrieve information.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext ctx)
        {
            if (ctx.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {                
                if (!ctx.ApiDescription.CustomAttributes().Any((a) => a is AllowAnonymousAttribute)
                    && (ctx.ApiDescription.CustomAttributes().Any((a) => a is AuthorizeAttribute)
                        || descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>() != null))
                {
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                }
            }
        }
    }
}
