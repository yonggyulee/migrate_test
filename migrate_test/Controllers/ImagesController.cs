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

        public ImagesController()
        {
        }

        // GET: api/Images
        [HttpGet("{dataset_id}")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage(string dataset_id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                return await ldmdb.Image.ToListAsync();
            }
        }

        // GET: api/Images/5
        [HttpGet("{dataset_id}/{id}")]
        public async Task<ActionResult<Image>> GetImage(string dataset_id, string id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                var image = await ldmdb.Image.FindAsync(id);

                if (image == null)
                {
                    return NotFound();
                }

                return image;
            }
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{dataset_id}/{id}")]
        public async Task<IActionResult> PutImage(string dataset_id, string id, Image image)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                if (id != image.ImageID)
                {
                    return BadRequest();
                }

                ldmdb.Entry(image).State = EntityState.Modified;

                try
                {
                    await ldmdb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(ldmdb, id))
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
        }

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{dataset_id}")]
        public async Task<ActionResult<Image>> PostImage(string dataset_id, Image image)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                ldmdb.Image.Add(image);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ImageExists(ldmdb, image.ImageID))
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
        }

        // DELETE: api/Images/5
        [HttpDelete("{dataset_id}/{id}")]
        public async Task<IActionResult> DeleteImage(string dataset_id, string id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                var image = await ldmdb.Image.FindAsync(id);
                if (image == null)
                {
                    return NotFound();
                }

                ldmdb.Image.Remove(image);
                await ldmdb.SaveChangesAsync();

                return NoContent();
            }
        }

        private bool ImageExists(LDMContext ldmdb, string id)
        {
            return ldmdb.Image.Any(e => e.ImageID == id);
        }
    }
}
