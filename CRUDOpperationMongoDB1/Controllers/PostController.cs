
using CRUDOpperationMongoDB1.Application.Command.Post;
using CRUDOpperationMongoDB1.Application.Queries.PostQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        var postId = await _mediator.Send(command);
        return Ok(new { Message = "Post created successfully!", PostId = postId });
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) return NotFound(new { Message = "Post not found!" });

        return Ok(new { Message = "Post updated successfully!" });
    }
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetPostBySlug(string slug)
    {
        var post = await _mediator.Send(new GetPostBySlugQuery(slug));
        if (post == null) return NotFound(new { Message = "Post not found!" });
        return Ok(post);
    }
    [HttpPost("search")]
    public async Task<IActionResult> SearchPosts([FromBody] SearchPostsQuery request)
    {
        var result = await _mediator.Send(new SearchPostsQuery(
            request.keyWord,
            request.Page,
            request.PageSize
        ));
        return Ok(result);
    }
}
