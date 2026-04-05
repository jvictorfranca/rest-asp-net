namespace RestASPNet.Repositories.QueryBuilders
{
    public class PersonQueryBuilder
    {
        public (string query,
    string countQuery,
    string sort,
    int size,
    int offset
    ) BuildQueries(string name, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(page, 1);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;

            var sort = !string.IsNullOrEmpty(sortDirection) && !sortDirection.ToLower().Equals("desc") ? "asc" : "desc";

            var baseQuery = $"FROM person p WHERE 1=1 ";
            if (!string.IsNullOrEmpty(name))
            {
                baseQuery += $"AND (p.first_name LIKE '%{name}%' OR p.last_name LIKE '%{name}%') ";

            }
            var query = $@"
                SELECT * {baseQuery} 
                ORDER BY p.first_name {sort} 
                OFFSET {offset} ROWS 
                FETCH NEXT {size} ROWS ONLY
                ";

            var countQuery = $"SELECT COUNT(*) {baseQuery}";

            return (query, countQuery, sort, size, offset);

        }
    }
}
