using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Hypermedia.Constants;

namespace RestASPNet.Hypermedia.Enricher
{
    public class PersonEnricher : ContentResponseEnricher<PersonDTO>
    {
        protected override Task EnrichModel(PersonDTO content, IUrlHelper urlHelper)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}";
            content.Links.AddRange(GenerateLinks(content.Id, baseUrl));
        }

        private IEnumerable<HypermediaLink> GenerateLinks(long id, string baseUrl)
        {
            return new List<HypermediaLink>
            {
                new HypermediaLink
                {
                    Rel = RelationType.COLLECTION,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultGet,
                    Action = HttpActionVerb.GET,
                },
                new HypermediaLink
                {
                    Rel = RelationType.SELF,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultGet,
                    Action = HttpActionVerb.GET,
                },
                new HypermediaLink
                {
                    Rel = RelationType.CREATE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultPost,
                    Action = HttpActionVerb.PUT,
                },
                new HypermediaLink
                {
                    Rel = RelationType.UPDATE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultPut,
                    Action = HttpActionVerb.PUT,
                },
                new HypermediaLink
                {
                    Rel = RelationType.PATCH,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultPatch,
                    Action = HttpActionVerb.PATCH,
                },
                new HypermediaLink
                {
                    Rel = RelationType.DELETE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.defaultDelete,
                    Action = HttpActionVerb.DELETE,
                },
            };
        }
    }
}
