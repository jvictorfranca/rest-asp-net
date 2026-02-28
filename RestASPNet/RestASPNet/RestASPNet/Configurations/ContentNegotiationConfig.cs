using System.Net.Http.Headers;

namespace RestASPNet.Configurations
{
    public static class ContentNegotiationConfig
    {
        public static IMvcBuilder AddContentNegotiation(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
            })
                
                
                .AddXmlSerializerFormatters();
        }
    }
}
