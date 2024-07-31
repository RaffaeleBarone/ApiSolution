using DataAccess;
using DataAccess.Entities;
using JsonPlaceholderApiClient;
using JsonPlaceholderDataAccess.Entities;
using JsonPlaceholderWebApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommentsController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public CommentsController(IApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Import comments data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-comments")]
        public async Task<IActionResult> ImportComments()
        {
            try
            {
                var comments = await _client.GetAsync<Comments>("https://jsonplaceholder.typicode.com/comments");
                if (comments == null || !comments.Any())
                {
                    throw new NotFoundException("Nessun commento da importare");
                }

                _context.Comments.AddRange(comments);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"Errore nell'import dei commenti: {ex.Message}");
            }
        }

        /// <summary>
        /// Return all comments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            //var comments = await _context.Comments.ToListAsync();
            var comments = await _client.GetAsync<Comments>("https://jsonplaceholder.typicode.com/comments");

            return Ok(comments);
        }

        /// <summary>
        /// Return comments by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetCommentsById(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                throw new NotFoundException($"Commento con ID {id} non trovato");
            }

            return Ok(comments);
        }

        /// <summary>
        /// Return comments by PostId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Comments>>> SearchComments([FromQuery] int postId)
        {
            if (postId <= 0)
            {
                throw new BadRequestException("Invalid post ID.");
            }
            var comments = await _context.Comments.Where(c => c.postID == postId).ToListAsync();
            if (comments == null || !comments.Any())
            {
                throw new NotFoundException($"Nessun commento trovato con postId: {postId}.");
            }

            return Ok(comments);
        }
    }
}
