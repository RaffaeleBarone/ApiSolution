using DataAccess;
using JsonPlaceholderApiClient;
using JsonPlaceholderDataAccess.Entities;
using JsonPlaceholderWebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JsonPlaceholderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public UsersController(IApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }

        /// <summary>
        /// Import users data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-users")]
        public async Task<IActionResult> ImportUsers()
        {
            try
            {
                var users = await _client.GetUsersAsync();
                if (users == null || !users.Any())
                {
                    throw new NotFoundException("Nessun utente da importare");
                }
                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException($"Errore nell'import dei post: {ex.Message}");
            }
        }

        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Return users by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User con ID {id} non trovato");
            }

            return Ok(user);
        }

        /// <summary>
        /// Modifies users by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Users.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deelte users by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User con ID {id} non trovato");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
