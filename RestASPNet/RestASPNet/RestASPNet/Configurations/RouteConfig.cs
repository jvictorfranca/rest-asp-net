using System.Runtime.CompilerServices;

namespace RestASPNet.Configurations
{
    public static class RouteConfig
    {
        public static IServiceCollection AddRouterConfig(
            this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;

            });
            return services;
        }
    }
}
