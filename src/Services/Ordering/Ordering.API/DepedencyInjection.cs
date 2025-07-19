namespace Ordering.API
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app) 
        {
            return app;
        }
    }
}
