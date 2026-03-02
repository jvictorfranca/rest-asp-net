namespace RestASPNet.Configurations
{
    public static class CORSConfig
    {
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("LocalPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") //.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });

                options.AddPolicy("MultipleOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://example.com") //.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
        }
        public static IApplicationBuilder UseCorsConfiguration(this WebApplication app)
        {
            app.UseCors();
            return app;
        }
    }
}
