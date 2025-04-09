using CRUDOpperationMongoDB1.Models;
using CRUDOpperationMongoDB1.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDOpperationMongoDB1.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
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
        // Create post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO postDto)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (postDto == null || string.IsNullOrWhiteSpace(postDto.Title))
                {
                    return BadRequest("Dữ liệu không hợp lệ. Tiêu đề không được để trống.");
                }

                // Gọi service để tạo post
                var post = await _postService.CreatePostAsync(postDto);

                // Trả về response với location của post mới tạo
                return CreatedAtAction(
                    nameof(GetPostSlug),
                    new { slug = post.Slug },
                    post
                );
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Lỗi khi tạo bài viết: {ex.Message}");
            }
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
