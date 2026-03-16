using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestASPNet.Hypermedia.Abstract;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace RestASPNet.Hypermedia
{
    public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportsHypermedia
    {

        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T) || contentType == typeof(List<T>);
        }

        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult)
            {

                return CanEnrich(okObjectResult.Value.GetType());
            }
            return false;
        }

        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
            if (response.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value is T content)
                {
                    await EnrichModel(content, urlHelper);
                }
                else if (okObjectResult.Value is List<T> contentList)
                {
                    foreach (var item in contentList)
                    {
                        await EnrichModel(item, urlHelper);
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}

