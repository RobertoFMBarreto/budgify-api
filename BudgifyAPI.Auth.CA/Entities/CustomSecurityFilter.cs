using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BudgifyAPI.Auth.CA.Entities;

public class CustomSecurityFilter: IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se a rota tem o atributo AllowAnonymous
        var hasAllowAnonymous = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<AllowAnonymousAttribute>().Any();

        if (hasAllowAnonymous)
        {
            // Remove o esquema de segurança se AllowAnonymous estiver presente
            operation.Security?.Clear();
        }
        else
        {
            // Adiciona o esquema de segurança se necessário
            operation.Security ??= new List<OpenApiSecurityRequirement>();
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
                    Array.Empty<string>()
                }
            });
        }
    }
}