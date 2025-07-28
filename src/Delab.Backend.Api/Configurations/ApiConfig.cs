using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Delab.AccessData.Context;
using Delab.AccessData.Data;
using Delab.Backend.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Delab.Backend.Api.Configurations;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddOpenApi();

        services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddControllers();

        services.AddSwaggerConfiguration();

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        services.AddCors(options => {
            options.AddPolicy("Cors", configure =>
            {
                configure.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod();
            });
        });
    }

    public static void UseApiConfiguration(this WebApplication app, IConfiguration configuration)
    {
        SeedData(app);

        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();                

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwaggerConfiguration(provider, configuration);

            string swaggerUrl = "https://localhost:7211/swagger";
            Task.Run(() => OpenBrowser(swaggerUrl));
        }

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();            

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("Cors");

        app.MapControllers();
    }

    private static void SeedData(WebApplication app)
    {
        IServiceScopeFactory? scopeFactory = app.Services.GetService<IServiceScopeFactory>();

        using (IServiceScope? scope = scopeFactory!.CreateScope())
        {
            SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
            service!.SeedAsync().Wait();
        }
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao abrir navegador: {ex.Message}");           
        }
    }
}
