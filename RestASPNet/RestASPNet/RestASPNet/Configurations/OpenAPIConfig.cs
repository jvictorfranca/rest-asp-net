using Microsoft.OpenApi;

namespace RestASPNet.Configurations
{
    public static class OpenAPIConfig
    {
        private static readonly string App_Name = "Asp net 2026 with Docker and Kubernetes";
        private static readonly string App_Description = "API RESTFUL developed on Udemy course";

        public static IServiceCollection AddOpenAPIConfig(this IServiceCollection services)
        {

            services.AddSingleton(new OpenApiInfo
            {
                Title = App_Name,
                Version = "v1",
                Description = App_Description,
                Contact = new OpenApiContact
                {
                    Name = "Joao Franca",
                    Email = "jvictorfranca@yahoo.com.br"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",

                }
            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            //    {
            //        Title = App_Name,
            //        Version = "v1",
            //        Description = App_Description
            //    });
            //});
            return services;
        }
    }
}
