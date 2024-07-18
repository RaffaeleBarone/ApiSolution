using DataAccess;
using DataAccess.Entities;
using JsonPlaceholderApiClient;
using JsonPlaceholderDataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApiClient _client;
        private readonly AppDbContext _context;

        public CommentsController(ApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        [HttpPost("import-comments")]
        public async Task<IActionResult> ImportComments()
        {
            var comments = await _client.GetCommentsAsync();
            _context.Comments.AddRange(comments);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.Comments.ToListAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetCommentsById(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Comments>>> SearchComments([FromQuery] int postId)
        {
            var comments = await _context.Comments.Where(c => c.postID == postId).ToListAsync();
            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }

            return Ok(comments);
        }
    }
}
