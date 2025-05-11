using CRUDOpperationMongoDB1.Application.Queries.PostQuery;
using MediatR;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CRUDOpperationMongoDB1.Application.DTO;
namespace CRUDOpperationMongoDB1.Application.Handler.PostQueryHandlers
{
    public class SearchPostsQueryHandler : IRequestHandler<SearchPostsQuery, List<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public SearchPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<List<PostDto>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.SearchAsync(request.keyWord);
            if (posts == null || !posts.Any())
            {
                throw new Exception("Không tìm thấy bài viết nào phù hợp với từ khóa");
            }
            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}
