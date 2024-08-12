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
    public class CategoriasController : ControllerBase
    {
        private readonly BibliotecaIstlcContext _context;

        public CategoriasController(BibliotecaIstlcContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var list = await _context.Categorias.ToListAsync();

            return convierteaDTOCategoria(list);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> convierteaDTOCategoria(List<Categoria> list)
        {
            List<CategoriaDTO> result = new List<CategoriaDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                CategoriaDTO obj = new CategoriaDTO();
                var item = list[i];
                obj.IdCategoria = item.IdCategoria;
                obj.DescripcionCategoria = item.DescripcionCategoria;
                result.Add(obj);
            }
            return result;
        }



        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaDTO categoria)
        {
            Categoria result = transformaDTOaCategoria(categoria);
            if (id != categoria.IdCategoria)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        private Categoria transformaDTOaCategoria(CategoriaDTO categoria)
        {
            Categoria obj = new Categoria();
            obj.IdCategoria = categoria.IdCategoria;
            obj.DescripcionCategoria = categoria.DescripcionCategoria;
            obj.Estado = "A";

            return obj;
        }

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(CategoriaDTO categoria)
        {
            Categoria result = transformaDTOaCategoria(categoria);
            _context.Categorias.Add(result);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoriaExists(categoria.IdCategoria))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategoria", new { id = categoria.IdCategoria }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.IdCategoria == id);
        }
    }
}
