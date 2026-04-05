namespace RestASPNet.Configurations
{
    public static class CORSConfig
    {
        private static string[] GetAllowedOrigins(IConfiguration configuration)
        {
            return configuration.GetSection("CORS:Origins").Get<string[]>() ?? Array.Empty<string>();
        }
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = GetAllowedOrigins(configuration);
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

                options.AddPolicy("DefaultPolicy", builder =>
                {
                    builder.WithOrigins(origins) //.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
        }
        public static IApplicationBuilder UseCorsConfiguration(this WebApplication app, IConfiguration configuration)
        {
            var origins = GetAllowedOrigins(configuration);
            app.Use(async (context, next) =>
            {
                var selfOrigin = $"{context.Request.Scheme}://{context.Request.Host}";
                var origin = context.Request.Headers["Origin"].ToString();
                if (
                !string.IsNullOrEmpty(origin) 
                && !origins.Contains(origin, StringComparer.OrdinalIgnoreCase) 
                && !origin.Equals(selfOrigin, StringComparison.OrdinalIgnoreCase)
                )
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("CORS policy does not allow this origin.");
                return;

                }
                else
                    await next();
            });
            // app.UseCors(); // Use every policy
            app.UseCors("DefaultPolicy"); // Use the default policy defined in AddCorsConfiguration
            return app;
        }
    }
}
