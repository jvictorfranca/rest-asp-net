using RestASPNet.Hypermedia;
using RestASPNet.Hypermedia.Abstract;

namespace RestASPNet.Data.DTO.V1
{
    public class BookDTO : ISupportsHypermedia
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public decimal Price { get; set; }

 
        public DateTime LaunchDate { get; set; }
        public List<HypermediaLink> Links { get; set; } = [];
    }
}
