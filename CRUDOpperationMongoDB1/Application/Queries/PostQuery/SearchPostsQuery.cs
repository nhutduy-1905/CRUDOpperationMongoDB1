using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Queries.PostQuery
{
    public class SearchPostsQuery : IRequest<List<PostDto>>
    {
        public string? keyWord { get; set; } // tu khoa tim kiem theo tiltle

        public SearchPostsQuery(string keyWord)
        {
            this.keyWord = keyWord;
        }
    }
}
