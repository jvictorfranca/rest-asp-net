using RestASPNet.Mail.Settings;

namespace RestASPNet.Configurations
{
    public static class EmailConfig
    {
        public static IServiceCollection AddEmailConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Email");
            var configs = section.Get<EmailSettings>();

            if (configs == null)
            {
                throw new ArgumentException(nameof(configs), "Email configuration is missing or invalid.");
            }



            configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? configs.Username;

            configs.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? configs.Password;

            services.AddSingleton(configs);

            return services;

        }
    }
}
