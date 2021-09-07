﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using migrate_test.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace migrate_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly LDMContext _context;

        public SamplesController()
        {
        }

        // GET: api/Samples/dataset_id
        [HttpGet("{dataset_id}")]
        public async Task<ActionResult<IEnumerable<Sample>>> GetSample(string dataset_id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                return await ldmdb.Sample.ToListAsync();
            }
        }

        // GET: api/Samples/dataset_id/5
        [HttpGet("{dataset_id}/{id}")]
        public async Task<ActionResult<Sample>> GetSample(string dataset_id, int id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                var sample = await ldmdb.Sample.FindAsync(id);

                if (sample == null)
                {
                    return NotFound();
                }

                return sample;
            }
        }

        // PUT: api/Samples/dataset_id/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{dataset_id}/{id}")]
        public async Task<IActionResult> PutSample(string dataset_id, int id, Sample sample)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                if (id != sample.SampleID)
                {
                    return BadRequest();
                }

                ldmdb.Entry(sample).State = EntityState.Modified;

                try
                {
                    await ldmdb.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SampleExists(ldmdb, id))
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

        // POST: api/Samples/dataset_id
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{dataset_id}")]
        public async Task<ActionResult<Sample>> PostSample(string dataset_id, Sample sample)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                Console.WriteLine($"SAMPLE POST ADD : {dataset_id} - {sample.SampleID}");
                ldmdb.Sample.Add(sample);
                Console.WriteLine($"SAMPLE POST SAVE : {dataset_id} - {sample.SampleID}");
                await ldmdb.SaveChangesAsync();

                return CreatedAtAction("GetSample", new { dataset_id = dataset_id, id = sample.SampleID }, sample);
            }
        }

        // DELETE: api/Samples/dataset_id/5
        [HttpDelete("{dataset_id}/{id}")]
        public async Task<IActionResult> DeleteSample(string dataset_id, int id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                var sample = await ldmdb.Sample.FindAsync(id);
                if (sample == null)
                {
                    return NotFound();
                }

                ldmdb.Sample.Remove(sample);
                await ldmdb.SaveChangesAsync();

                return NoContent();
            }
        }

        private bool SampleExists(LDMContext ldmdb, int id)
        {
            return ldmdb.Sample.Any(e => e.SampleID == id);
        }
    }
}
