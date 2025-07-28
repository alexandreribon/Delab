using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Delab.Backend.Api.Configurations;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<SwaggerDefaultValues>();

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey, //ApiKey: Bearer {seu token} | Http: {seu token} na hora de passar o token
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, new string[]{} }
            });
        });
    }

    public static void UseSwaggerConfiguration(this WebApplication app, IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        var vPersonalised = Convert.ToBoolean(configuration["CustomSwaggerUi:Personalised"]);

        app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                //c.RoutePrefix = string.Empty;

                if (vPersonalised)
                {
                    c.DocumentTitle = configuration["CustomSwaggerUi:DocTitle"];
                    c.HeadContent = configuration["CustomSwaggerUi:HeaderImg"];
                    c.InjectStylesheet(configuration["CustomSwaggerUi:PathCss"]);
                }
            }
        });
    }
}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Projeto Delab API",
            Version = description.ApiVersion.ToString(),
            Description = "Projeto Delab API 2025",
            Contact = new OpenApiContact() { Name = "Alexandre Ribon", Email = "xribon@gmail.com" },
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " Esta versão está obsoleta!";
        }

        return info;
    }
}

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            if (parameter.Description == null)
            {
                parameter.Description = description.ModelMetadata?.Description;
            }

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                //OpenApiAnyFactory.CreateFor(parameter.Schema, description.DefaultValue);
            }

            parameter.Required |= description.IsRequired;
        }

    }
}
