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
    public class PhotosController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public PhotosController(IApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Import photos data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-photos")]
        public async Task<IActionResult> ImportPhotos()
        {
            var photos = await _client.GetPhotosAsync();
            _context.Photos.AddRange(photos);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Return all photos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var photos = await _context.Photos.ToListAsync();
            return Ok(photos);
        }

        /// <summary>
        /// Return photos by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Photos>> GetPhotosById(int id)
        {
            var photos = await _context.Photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }

            return Ok(photos);
        }

        /// <summary>
        /// Return comments by AlbumId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Photos>>> SearchPost([FromQuery] int albumId)
        {
            var photos = await _context.Photos.Where(p => p.AlbumId == albumId).ToListAsync();

            if (photos == null || photos.Count == 0)
            {
                return NotFound();
            }

            return Ok(photos);
        }
    }
}
 