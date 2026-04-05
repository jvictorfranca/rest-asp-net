using RestASPNet.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace RestASPNet.Hypermedia.Utils
{
    public class FilterItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class PagedSearchDTO<T> where T : ISupportsHypermedia
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }

        public string sortFields { get; set; }
        public string sortDirection { get; set; } = "asc";
        [XmlIgnore]
        public List<FilterItem> Filters { get; set; } = [];
        public List<T> List { get; set; } = [];
    
        public PagedSearchDTO()
        {}
        public int GetCurrentPage()
        {
            return CurrentPage == 0 ? 1 : CurrentPage;
        }

        public int getPageSize()
        {
            return PageSize == 0 ? 10 : PageSize;
        }
    }


}
