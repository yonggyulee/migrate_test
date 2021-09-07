using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using migrate_test.Models;

namespace migrate_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly LDMContext _context;

        public ImagesController(LDMContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage()
        {
            return await _context.Image.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(string id)
        {
            var image = await _context.Image.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(string id, Image image)
        {
            if (id != image.ImageID)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
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

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage(Image image)
        {
            _context.Image.Add(image);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ImageExists(image.ImageID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImage", new { id = image.ImageID }, image);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(string id)
        {
            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Image.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(string id)
        {
            return _context.Image.Any(e => e.ImageID == id);
        }
    }
}
