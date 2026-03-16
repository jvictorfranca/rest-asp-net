using Microsoft.AspNetCore.Mvc;

namespace RestASPNet.Hypermedia.Utils
{
    public static class UrlHelper
    {
        private static readonly object _lock = new();
        public static string BuildBaseUrl(this IUrlHelper urlHelper, string routeName, string path)
        {
            lock (_lock)
            {
                var baseUrl = urlHelper.Link(routeName, new { controller = path }) ?? string.Empty;
                
                return baseUrl.Replace("%2F", "/").TrimEnd('/');
            }
        }
    }
}
