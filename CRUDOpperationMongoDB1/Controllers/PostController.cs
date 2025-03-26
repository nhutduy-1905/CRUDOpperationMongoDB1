using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
        }
        // Create post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
        {
            if (postDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var post = await _postService.CreatePostAsync(postDto);
            return CreatedAtAction(nameof(GetPostSlug), new{slug = post.Slug },post);
        }
        // update post
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] CreatePostDTO postDto)
        {
            if (postDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var updatedPost = await _postService.UpdatePostAsync(id, postDto);
            if (updatedPost == null)
            {
                return NotFound("Không tìm thấy bài viết.");
            }
            return Ok(updatedPost);
        }
        // Get Post By Slug
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPostSlug(string slug)
        {
            var post = await _postService.GetPostBySlugAsync(slug);
            if (post == null)
            {
                return NotFound("Không tìm thấy bài viết.");
            }
            return Ok(post);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var deleted = await _postService.DeletePostAsync(id); 
            if (!deleted)
            {
                return NotFound(new { message =  "Không tìm thấy bài viết để xóa." });
            }
            return Ok(new { message = "Bài viết đã được xóa thành công." });
        }
    }
}
