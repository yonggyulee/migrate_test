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
    public class SamplesController : ControllerBase
    {
        private readonly LDMContext _context;

        public SamplesController(LDMContext context)
        {
            _context = context;
        }

        // GET: api/Samples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sample>>> GetSample()
        {
            return await _context.Sample.ToListAsync();
        }

        // GET: api/Samples/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sample>> GetSample(int id)
        {
            var sample = await _context.Sample.FindAsync(id);

            if (sample == null)
            {
                return NotFound();
            }

            return sample;
        }

        // PUT: api/Samples/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSample(int id, Sample sample)
        {
            if (id != sample.SampleID)
            {
                return BadRequest();
            }

            _context.Entry(sample).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SampleExists(id))
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

        // POST: api/Samples
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sample>> PostSample(Sample sample)
        {
            _context.Sample.Add(sample);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSample", new { id = sample.SampleID }, sample);
        }

        // DELETE: api/Samples/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSample(int id)
        {
            var sample = await _context.Sample.FindAsync(id);
            if (sample == null)
            {
                return NotFound();
            }

            _context.Sample.Remove(sample);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SampleExists(int id)
        {
            return _context.Sample.Any(e => e.SampleID == id);
        }
    }
}
