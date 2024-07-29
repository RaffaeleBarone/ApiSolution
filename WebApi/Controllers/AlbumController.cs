using DataAccess;
using JsonPlaceholderApiClient;
using JsonPlaceholderDataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AlbumController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public AlbumController(IApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Import albums data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-albums")]
        public async Task<IActionResult> ImportAlbums()
        {
            var albums = await _client.GetAlbumsAsync();
            _context.Albums.AddRange(albums);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Return all albums
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await _context.Albums.ToListAsync();
            return Ok(albums);
        }

        /// <summary>
        /// Return albums by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("post/{id}")]
        public async Task<ActionResult<Albums>> GetAlbumsById(int id)
        {
            var albums = await _context.Albums.FindAsync(id);
            if (albums == null)
            {
                return NotFound();
            }

            return Ok(albums);
        }

        /// <summary>
        /// Return albums by UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Albums>>> SearchPost([FromQuery] int userId)
        {
            var albums = await _context.Albums.Where(c => c.userId == userId).ToListAsync();

            if (albums == null || albums.Count == 0)
            {
                return NotFound();
            }

            return Ok(albums);
        }
    }
}
