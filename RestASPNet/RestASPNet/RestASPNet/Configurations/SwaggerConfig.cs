using Microsoft.OpenApi;

namespace RestASPNet.Configurations
{
    public static class SwaggerConfig
    {
        private static readonly string App_Name = "Asp net 2026 with Docker and Kubernetes";
        private static readonly string App_Description = "API RESTFUL developed on Udemy course";

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = App_Name,
                    Version = "v1",
                    Description = App_Description,
                    Contact = new OpenApiContact
                    {
                        Name = "Joao Franca",
                        Email = "jvictorfranca@yahoo.com.br"
                    },
                });
                options.CustomSchemaIds(type => type.FullName);
            });
            return services;
        }
    public static IApplicationBuilder UseSwaggerSpecification(this IApplicationBuilder app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger-ui";
                options.DocumentTitle = "Swagger UI - RestASPNet";
            });
        return app;
        }
    };

}
