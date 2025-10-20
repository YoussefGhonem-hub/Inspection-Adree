namespace Inspection.Helpers.Middlewares.ExceptionHandling;

public static class ServicesRegistrationExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
        return services;
    }

    public static WebApplication UseExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}
