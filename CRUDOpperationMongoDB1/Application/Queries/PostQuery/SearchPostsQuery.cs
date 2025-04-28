using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Queries.PostQuery
{
    public class SearchPostsQuery : IRequest<List<PostDto>>
    {
        public string? keyWord { get; set; } // tu khoa tim kiem theo tiltle
        public int Page { get; set; } = 1;// trang hien tai
        public int PageSize { get; set; } = 10; // so luong item moi trang

        public SearchPostsQuery(string? keyWord, int page, int pageSize)
        {
            keyWord = keyWord;
            Page = page;
            PageSize = pageSize;
        }
    }
}
