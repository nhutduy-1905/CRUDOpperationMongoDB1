using CRUDOpperationMongoDB1.Application.Command.Post;
using MediatR;

namespace CRUDOpperationMongoDB1.Application.Handler.PostCommandHandlers
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
    {
        private readonly IPostRepository _postRepository;

        public UpdatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);
            if (post == null) return false;

            post.Title = request.Title ?? post.Title;
            post.Content = request.Content ?? post.Content;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);
            return true;
        }
    }
}
