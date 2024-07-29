﻿using DataAccess;
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
    public class TodosController : ControllerBase
    {
        private readonly IApiClient _client;
        private readonly AppDbContext _context;

        public TodosController(IApiClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }


        /// <summary>
        /// Import todos data on db
        /// </summary>
        /// <returns></returns>
        [HttpPost("import-todos")]
        public async Task<IActionResult> ImportTodos()
        {
            var todos = await _client.GetTodosAsync();
            _context.Todos.AddRange(todos);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Return all todos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var todos = await _context.Todos.ToListAsync();
            return Ok(todos);
        }

        /// <summary>
        /// Return todos by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Todos>> GetTodoById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        /// <summary>
        /// Modifies todos by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, Todos todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Todos.AnyAsync(e => e.Id == id))
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
        /// Deelte todos by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
