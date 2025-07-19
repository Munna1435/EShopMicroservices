using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}


