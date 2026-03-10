using RestASPNet.Hypermedia.Abstract;

namespace RestASPNet.Hypermedia.Filters
{
    public class HypermediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = [];
    }
}
