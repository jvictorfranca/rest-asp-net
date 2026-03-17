using RestASPNet.Hypermedia.Enricher;
using RestASPNet.Hypermedia.Filters;
using System.Net.NetworkInformation;

namespace RestASPNet.Configurations
{
    public static class HATEOASConfig
    {
        public static IServiceCollection AddHATEOASConfiguration(this IServiceCollection services)
        {
            var filterOptions = new HypermediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            services.AddSingleton(filterOptions);

            services.AddScoped<HypermediaFilter>();
        }

        public static void UseHATEOASRoutes(this IEndpointRouteBuilder app)
        {
            app.MapControllerRoute("Default", "{controller=values}/v1/{id?}");
        }
    }
}
