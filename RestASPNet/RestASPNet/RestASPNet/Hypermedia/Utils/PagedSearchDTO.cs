using RestASPNet.Hypermedia.Abstract;

namespace RestASPNet.Hypermedia.Utils
{
    public class PagedSearchDTO<T> where T : ISupportsHypermedia
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }

        public string sortFields { get; set; }
        public string sortDirection { get; set; } = "asc";
        public Dictionary<string, object> Filters { get; set; } = [];
        public List<T> List { get; set; } = [];
    
        public PagedSearchDTO(int currentPage, int pageSize, string sortFields, string sortDirection, Dictionary<string, object> filters)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            this.sortFields = sortFields;
            this.sortDirection = sortDirection;
            Filters = filters ?? [];
        }
        public PagedSearchDTO(int currentPage, string sortFields, string sortDirection)
            : this(currentPage, 10, sortFields, sortDirection, null) { }

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
