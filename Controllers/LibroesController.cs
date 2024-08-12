using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaISTLC.Context;
using BibliotecaISTLC.Models;
using BibliotecaISTLC.DTO;

namespace BibliotecaISTLC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroesController : ControllerBase
    {
        private readonly BibliotecaIstlcContext _context;

        public LibroesController(BibliotecaIstlcContext context)
        {
            _context = context;
        }

        // GET: api/Libroes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibros()
        {
            var list = await _context.Libros.ToListAsync();

            return convierteaDTOLibro(list);
        }

        private ActionResult<IEnumerable<LibroDTO>> convierteaDTOLibro(List<Libro> list)
        {
            List<LibroDTO> result = new List<LibroDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                LibroDTO obj = new LibroDTO();
                var item = list[i];
                obj.IdLibros = item.IdLibros;
                obj.Nombre = item.Nombre;
                obj.IdCategoria = item.IdCategoria;
                obj.IdAutor = item.IdAutor;
                obj.IdEditorial = item.IdEditorial;
                result.Add(obj);
            }
            return result;
        }

        // GET: api/Libroes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // PUT: api/Libroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, LibroDTO libro)
        {
            Libro result = transformaDTOaLibro(libro);
            if (id != libro.IdLibros)
            {
                return BadRequest();
            }

            _context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(id))
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

        private Libro transformaDTOaLibro(LibroDTO libro)
        {
            Libro obj = new Libro();
            obj.IdLibros = libro.IdLibros;
            obj.Nombre = libro.Nombre;
            obj.IdCategoria = libro.IdCategoria;
            obj.IdAutor = libro.IdAutor;
            obj.IdEditorial = libro.IdEditorial;
            obj.Estado = "A";

            return obj;
        }

        // POST: api/Libroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(LibroDTO libro)
        {
            Libro result = transformaDTOaLibro(libro);
            _context.Libros.Add(result);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LibroExists(libro.IdLibros))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLibro", new { id = libro.IdLibros }, libro);
        }

        // DELETE: api/Libroes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.IdLibros == id);
        }
    }
}
