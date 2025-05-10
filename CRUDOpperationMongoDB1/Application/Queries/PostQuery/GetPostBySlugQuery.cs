using CRUDOpperationMongoDB1.Application.DTO;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Queries.PostQuery
{
    public class GetPostBySlugQuery :IRequest<PostDto>
    {
        public string Slug { get; set; }
        public GetPostBySlugQuery(string slug)
        {
            Slug = slug;
        }
    }
}
