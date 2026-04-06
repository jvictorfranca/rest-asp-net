using RestASPNet.Repositories.QueryBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestASPNet.Tests.UnitTests
{
    public class PersonQueryBuilderTests
    {
        private readonly PersonQueryBuilder _queryBuilder;

        public PersonQueryBuilderTests()
        {
            _queryBuilder = new PersonQueryBuilder();
        }

        [Fact]
        public void BuildQueries_ShouldReturnCorrectQueryAndCountQuery()
        {
            // Arrange
            string name = "John";
            string sortDirection = "asc";
            int pageSize = 10;
            int page = 2;
            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
            // Assert
            Assert.Contains("FROM person p WHERE 1=1", query);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", query);
            Assert.Contains("ORDER BY p.first_name asc", query);
            Assert.Contains("OFFSET 10 ROWS", query);
            Assert.Contains("FETCH NEXT 10 ROWS ONLY", query);
            Assert.Contains("SELECT COUNT(*) FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Equal("asc", sort);
            Assert.Equal(10, size);
            Assert.Equal(10, offset);

            Assert.Contains("FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Contains("SELECT COUNT(*)", countQuery);
        }

        [Fact]
        public void BuildQueries_ShouldHandleInvalidSize()
        {
            // Arrange
            string name = "John";
            string sortDirection = "asc";
            int pageSize = -1;
            int page = -2;
            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
            // Assert
            Assert.Contains("FROM person p WHERE 1=1", query);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", query);
            Assert.Contains("ORDER BY p.first_name asc", query);
            Assert.Contains("OFFSET 0 ROWS", query);
            Assert.Contains("FETCH NEXT 1 ROWS ONLY", query);
            Assert.Contains("SELECT COUNT(*) FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Equal("asc", sort);
            Assert.Equal(1, size);
            Assert.Equal(0, offset);

            Assert.Contains("FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Contains("SELECT COUNT(*)", countQuery);
        }

        [Fact]
        public void BuildQueries_ShouldHandleInvalidSort()
        {
            // Arrange
            string name = "John";
            string sortDirection = "Invalid";
            int pageSize = 10;
            int page = 2;
            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
            // Assert
            Assert.Contains("FROM person p WHERE 1=1", query);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", query);
            Assert.Contains("ORDER BY p.first_name asc", query);
            Assert.Contains("OFFSET 10 ROWS", query);
            Assert.Contains("FETCH NEXT 10 ROWS ONLY", query);
            Assert.Contains("SELECT COUNT(*) FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Equal("asc", sort);
            Assert.Equal(10, size);
            Assert.Equal(10, offset);

            Assert.Contains("FROM person p WHERE 1=1", countQuery);
            Assert.Contains("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Contains("SELECT COUNT(*)", countQuery);
        }


        [Fact]
        public void BuildQueries_ShouldReturnCorrectQueryAndCountQueryWithoutNameParameter()
        {
            // Arrange
            string sortDirection = "asc";
            int pageSize = 10;
            int page = 2;
            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries("", sortDirection, pageSize, page);
            // Assert
            Assert.Contains("FROM person p WHERE 1=1", query);
            Assert.DoesNotContain("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", query);
            Assert.Contains("ORDER BY p.first_name asc", query);
            Assert.Contains("OFFSET 10 ROWS", query);
            Assert.Contains("FETCH NEXT 10 ROWS ONLY", query);
            Assert.Contains("SELECT COUNT(*) FROM person p WHERE 1=1", countQuery);
            Assert.DoesNotContain("AND (p.first_name LIKE '%John%' OR p.last_name LIKE '%John%')", countQuery);
            Assert.Equal("asc", sort);
            Assert.Equal(10, size);
            Assert.Equal(10, offset);
        }
    }
}
