using Inspection.Application;
using Inspection.Helpers.Middlewares.ExceptionHandling;
using Inspection.Infrastructure;
using Inspection.Infrastructure.Persistence;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace Inspection.Helpers;
public static class ProgramExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Project Services
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices();
        #endregion


        #region .NET Services
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddExceptionHandling();
        #endregion

        #region Swagger

        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Inspection System", Version = "v1" });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            });
        });
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        #endregion


        return services;
    }

    public static WebApplication UserServices(this WebApplication app)
    {
        app.UseCors(); // Apply the default CORS policy globally
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapsed view
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var groupName in provider?.ApiVersionDescriptions?
            .Select(x => x.GroupName).ToList() ?? [])
            {
                options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
                   groupName.ToUpperInvariant());
            }
        });

        var supportedCultures = new[] { "en", "ar" }
               .Select(c => new CultureInfo(c)).ToList();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
        // Add Middleware before any authentication processing
        app.UseRouting();

        app.UseExceptionHandling();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseStaticFiles();

        return app;
    }
    public static async Task MigrateAndSeedData(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        await ConfigurationDbContextSeed.InitializeDatabase(dbContext);
    }
}