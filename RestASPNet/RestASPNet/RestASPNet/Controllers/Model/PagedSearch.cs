using RestASPNet.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace RestASPNet.Controllers.Model
{
    public class FilterItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class PagedSearch<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }

        public string sortFields { get; set; }
        public string sortDirection { get; set; } = "asc";
        [XmlIgnore]
        public List<FilterItem> Filters { get; set; } = [];
        public List<T> List { get; set; } = [];
    }

}
