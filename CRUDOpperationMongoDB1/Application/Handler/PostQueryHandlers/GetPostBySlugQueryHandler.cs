using CRUDOpperationMongoDB1.Application.Queries.PostQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using CRUDOpperationMongoDB1.Application.DTO;
namespace CRUDOpperationMongoDB1.Application.Handler.PostQueryHandlers
{
    public class GetPostBySlugQueryHandler : IRequestHandler<GetPostBySlugQuery, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostBySlugQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<PostDto> Handle(GetPostBySlugQuery requets, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetBySlugAsync(requets.Slug);
            if (post == null) return null;

            return _mapper.Map<PostDto>(post);
           
        }
    }
}
