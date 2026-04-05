using RestASPNet.Hypermedia;
using RestASPNet.Hypermedia.Abstract;

namespace RestASPNet.Data.DTO.V1
{
    public class PersonDTO : ISupportsHypermedia
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Gender { get; set; }

        public bool Enabled { get; set; }
        public List<HypermediaLink>? Links { get; set; } = [];
    }
}
