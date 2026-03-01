using Scalar.AspNetCore;
using System.Runtime.CompilerServices;

namespace RestASPNet.Configurations
{
    public static class ScalarConfig
    {
        private static readonly string App_Name = "Asp net 2026 with Docker and Kubernetes";
        public static WebApplication UseScalarConfig(this WebApplication app)
        {
            app.MapScalarApiReference("/scalar", options =>
            {
                options.WithTitle(App_Name)
                .WithOpenApiRoutePattern("/swagger/v1/swagger.json");

            }
            );
                return app;
        }
    } }
