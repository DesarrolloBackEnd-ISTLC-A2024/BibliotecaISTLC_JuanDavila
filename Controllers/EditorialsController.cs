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
    public class EditorialsController : ControllerBase
    {
        private readonly BibliotecaIstlcContext _context;

        public EditorialsController(BibliotecaIstlcContext context)
        {
            _context = context;
        }

        // GET: api/Editorials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EditorialDTO>>> GetEditorials()
        {
            var list = await _context.Editorials.ToListAsync();

            return convierteaDTOEditorial(list);
        }

        private ActionResult<IEnumerable<EditorialDTO>> convierteaDTOEditorial(List<Editorial> list)
        {
            List<EditorialDTO> result = new List<EditorialDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                EditorialDTO obj = new EditorialDTO();
                var item = list[i];
                obj.IdEditorial = item.IdEditorial;
                obj.NombreEditorial = item.NombreEditorial;
                result.Add(obj);
            }
            return result;
        }

        // GET: api/Editorials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return editorial;
        }

        // PUT: api/Editorials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, EditorialDTO editorial)
        {
            Editorial result = transformaDTOaEditorial(editorial);
            if (id != editorial.IdEditorial)
            {
                return BadRequest();
            }

            _context.Entry(editorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        private Editorial transformaDTOaEditorial(EditorialDTO editorial)
        {
            Editorial obj = new Editorial();
            obj.IdEditorial = editorial.IdEditorial;
            obj.NombreEditorial = editorial.NombreEditorial;
            obj.Estado = "A";

            return obj;
        }

        // POST: api/Editorials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(EditorialDTO editorial)
        {
            Editorial result = transformaDTOaEditorial(editorial);
            _context.Editorials.Add(result);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EditorialExists(editorial.IdEditorial))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEditorial", new { id = editorial.IdEditorial }, editorial);
        }

        // DELETE: api/Editorials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }

            _context.Editorials.Remove(editorial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return _context.Editorials.Any(e => e.IdEditorial == id);
        }
    }
}
