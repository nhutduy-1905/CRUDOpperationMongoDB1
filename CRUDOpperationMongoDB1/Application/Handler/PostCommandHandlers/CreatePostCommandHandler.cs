using CRUDOpperationMongoDB1.Application.Command.Post;
using CRUDOpperationMongoDB1.Domain.Entities;
using MediatR;
using System.Reflection;
namespace CRUDOpperationMongoDB1.Application.Handler.PostCommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var slug = request.Title.ToLower().Replace(" ", "-");
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Author = request.Author,
                Slug = slug,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            return post.Id;
        }
    }
}
