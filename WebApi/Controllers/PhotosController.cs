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
    public class PhotosController : ControllerBase
    {
        private readonly ApiClient _client;
        private readonly AppDbContext _context;

        public PhotosController(ApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        [HttpPost("import-photos")]
        public async Task<IActionResult> ImportPhotos()
        {
            var photos = await _client.GetPhotosAsync();
            _context.Photos.AddRange(photos);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var photos = await _context.Photos.ToListAsync();
            return Ok(photos);
        }

        [HttpGet("post/{id}", Name = "GetCPhotosById")]
        public async Task<ActionResult<Photos>> GetPhotosById(int id)
        {
            var photos = await _context.Photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }

            return Ok(photos);
        }

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
 