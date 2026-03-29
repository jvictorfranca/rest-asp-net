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
            content.Links.AddRange(GenerateLinks(content.Id, urlHelper));
            return Task.CompletedTask;
        }

        private IEnumerable<HypermediaLink> GenerateLinks(long id,IUrlHelper urlHelper)
        {
            return new List<HypermediaLink>
            {
                new HypermediaLink
                {
                    Rel = RelationType.COLLECTION,
                    Href = urlHelper.Link("GetAllPersons", null),
                    Type = ResponseTypeFormat.defaultGet,
                    Action = HttpActionVerb.GET,
                },
                new HypermediaLink
                {
                    Rel = RelationType.COLLECTION,
                    Href = urlHelper.Link("GetAllPersonsByName", null),
                    Type = ResponseTypeFormat.defaultGet,
                    Action = HttpActionVerb.GET,
                },
                new HypermediaLink
                {
                    Rel = RelationType.SELF,
                    Href = urlHelper.Link("GetPersonById", new { id }),
                    Type = ResponseTypeFormat.defaultGet,
                    Action = HttpActionVerb.GET,
                },
                new HypermediaLink
                {
                    Rel = RelationType.CREATE,
                    Href = urlHelper.Link("CreatePerson", null),
                    Type = ResponseTypeFormat.defaultPost,
                    Action = HttpActionVerb.PUT,
                },
                new HypermediaLink
                {
                    Rel = RelationType.UPDATE,
                    Href = urlHelper.Link("UpdatePerson", null),
                    Type = ResponseTypeFormat.defaultPut,
                    Action = HttpActionVerb.PUT,
                },
                new HypermediaLink
                {
                    Rel = RelationType.PATCH,
                    Href = urlHelper.Link("DisablePerson", new { id }),
                    Type = ResponseTypeFormat.defaultPatch,
                    Action = HttpActionVerb.PATCH,
                },
                new HypermediaLink
                {
                    Rel = RelationType.DELETE,
                    Href = urlHelper.Link("DeletePerson", new { id }),
                    Type = ResponseTypeFormat.defaultDelete,
                    Action = HttpActionVerb.DELETE,
                },
            };
        }
    }
}
