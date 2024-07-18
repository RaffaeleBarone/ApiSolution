using DataAccess;
using DataAccess.Entities;
using JsonPlaceholderApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApiClient _client;
        private readonly AppDbContext _context;

        public PostController(ApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        [HttpPost("import-posts")]
        public async Task<IActionResult> ImportPosts()
        {
            var posts = await _client.GetPostsAsync();
            _context.Posts.AddRange(posts);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        [HttpGet("post/{id}", Name = "GetPostById")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Post>>> SearchPost([FromQuery] int userId)
        {
            var posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();  

            if(posts == null || posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts);
        }
    }
}
