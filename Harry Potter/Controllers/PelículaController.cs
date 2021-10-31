using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Harry_Potter.Modelos;

namespace Harry_Potter.Controllers
{
    [Route("api/pelicula")]
    [ApiController]
    public class PelículaController : ControllerBase
    {
        private readonly ContextoBD _context;

        public PelículaController(ContextoBD context)
        {
            _context = context;
        }

        // GET: api/Película
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Película>>> GetTodoItems()
        {
            return await _context.Películas.ToListAsync();
        }

        // GET: api/Película/Harry
        [HttpGet("{nombre}")]

        public List<Película> GetPelícula(string nombre)
        {
            var películaC = _context.Películas
                .Where(película => película.Title.Contains(nombre));
            return películaC.ToList();
        }

        // PUT: api/Película/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelícula(string id, Película película)
        {
            if (id != película.Id)
            {
                return BadRequest();
            }

            if (película.Valoración > 5)
            {
                película.Valoración = 5;
            }
            else if (película.Valoración < 1)
            {
                película.Valoración = 1;
            }
            _context.Entry(película).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PelículaExists(id))
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

        // POST: api/Película
        [HttpPost]
        public async Task<ActionResult<Película>> PostPelícula(Película película)
        {
            _context.Películas.Add(película);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PelículaExists(película.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPelícula", new { id = película.Id }, película);
        }

        private bool PelículaExists(string id)
        {
            return _context.Películas.Any(e => e.Id == id);
        }
    }
}
