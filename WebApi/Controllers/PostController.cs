using DataAccess;
using DataAccess.Entities;
using JsonPlaceholderApiClient;
using JsonPlaceholderWebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public PostController(IApiClient client, AppDbContext context) 
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Import posts data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-posts")]
        public async Task<IActionResult> ImportPosts()
        {
            try
            {
                var posts = await _client.GetAsync<Post>("https://jsonplaceholder.typicode.com/posts");
                if (posts == null || !posts.Any())
                {
                    throw new NotFoundException("Nessun post da importare");
                }
                _context.Posts.AddRange(posts);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"Errore nell'import dei post: {ex.Message}");
            }
        }

        /// <summary>
        /// Return all posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            //var posts = await _context.Posts.ToListAsync();
            var posts = await _client.GetAsync<Post>("https://jsonplaceholder.typicode.com/posts");
            return Ok(posts);
        }

        /// <summary>
        /// Return posts by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("post/{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                throw new NotFoundException($"Post con ID {id} non trovato");
            }

            return Ok(post);
        }

        /// <summary>
        /// Return comments by UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Post>>> SearchPost([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                throw new BadRequestException("Invalid user ID.");
            }

            var posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();

            if (posts == null || !posts.Any())
            {
                throw new NotFoundException($"Nessun post trovato con userId: {userId}.");
            }

            return Ok(posts);
        }

        /// <summary>
        /// Creates a Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns>A newly created Post</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Post
        ///     {
        ///        "id": 1,
        ///        "userID": 1,
        ///        "Title": ""
        ///        "Body": ""
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created post</response>
        /// <response code="400">If the post is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Create(Post post)
        {
            if (post == null)
            {
                throw new BadRequestException("Il post non può essere nullo");
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
