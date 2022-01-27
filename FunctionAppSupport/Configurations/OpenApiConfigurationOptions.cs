using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace FunctionAppSupport.Configurations;

public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = $"Cadastro de Chamados de Suporte",
        Description = "API de Chamados de Suporte implementada com .NET 6 + Azure Functions + MediatR + EF Core + SQL Server",
        Contact = new OpenApiContact()
        {
            Name = "Renato Groffe",
            Url = new Uri("https://github.com/renatogroffe"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
}