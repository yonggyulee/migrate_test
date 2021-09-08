using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using migrate_test.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace migrate_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        public ImagesController()
        {
        }

        // GET: api/Images/dataset_id
        [HttpGet("{dataset_id}")]
        public async Task<ActionResult<Object>> GetImage(string dataset_id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                return await ldmdb
                            .Image
                            .Select(i => new
                            {
                                i.ImageID,
                                i.SampleID,
                                i.ImageNO,
                                i.ImageCode,
                                i.OriginalFilename,
                                i.ImageScheme,
                                Sample = new
                                {
                                    i.Sample.SampleID,
                                    i.Sample.DatasetID,
                                    i.Sample.SampleType,
                                    i.Sample.Metadata,
                                    i.Sample.ImageCount
                                }
                            })
                            .ToListAsync();
            }
        }

        // GET: api/Images/dataset_id/5
        [HttpGet("{dataset_id}/{id}")]
        public async Task<Object> GetImage(string dataset_id, string id)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                Console.WriteLine($"GetImage(id) : {id}");
                var image = await ldmdb
                                .Image
                                .Select(i => new
                                {
                                    i.ImageID,
                                    i.SampleID,
                                    i.ImageNO,
                                    i.ImageCode,
                                    i.OriginalFilename,
                                    i.ImageScheme,
                                    Sample = new
                                    {
                                        i.Sample.SampleID,
                                        i.Sample.DatasetID,
                                        i.Sample.SampleType,
                                        i.Sample.Metadata,
                                        i.Sample.ImageCount
                                    }
                                })
                                .Where(i => i.ImageID == id)
                                .FirstAsync();

                Console.WriteLine($"GetImage(id) END : {id}");
                if (image == null)
                {
                    return NotFound();
                }

                return image;
            }
        }

        // PUT: api/Images/dataset_id/5
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

        // POST: api/Images/dataset_id
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{dataset_id}")]
        public async Task<ActionResult<Image>> PostImage(string dataset_id, Image image)
        {
            using (var ldmdb = new LDMContext(dataset_id))
            {
                //var sample = ldmdb.Sample.Find(image.SampleID);
                var sample = ldmdb.Sample.Include(b => b.Images).Where(s => s.SampleID == image.SampleID).First();
                Console.WriteLine($"POSTIMAGE FIND : {sample.SampleID}");

                Console.WriteLine($"POSTIMAGE sample add image : {image}");
                sample.Images.Add(image);
                sample.ImageCount = sample.Images.Count;
                
                //Console.WriteLine($"POSTIMAGE ADD IMAGE : {image.ImageID}");
                //ldmdb.Image.Add(image);

                //Console.WriteLine($"POSTIMAGE MODIFY SAMPLE : {sample}");
                //ldmdb.Entry(sample).State = EntityState.Modified;

                Console.WriteLine($"POSTIMAGE SAVE : {sample.SampleID}, {image.ImageID}");

                try
                {
                    await ldmdb.SaveChangesAsync();
                    Console.WriteLine($"POSTIMAGE SAVE COMPLETE : {sample.SampleID}, {image.ImageID}");
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
                catch (JsonException)
                {
                    Console.WriteLine("JSONEXCEPTION 발생");
                }

                return CreatedAtAction("GetImage", new { dataset_id = dataset_id, id = image.ImageID }, new
                {
                    image.ImageID,
                    image.SampleID,
                    image.ImageNO,
                    image.ImageCode,
                    image.OriginalFilename,
                    image.ImageScheme,
                    Sample = new
                    {
                        image.Sample.SampleID,
                        image.Sample.DatasetID,
                        image.Sample.SampleType,
                        image.Sample.Metadata,
                        image.Sample.ImageCount
                    }
                });
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

                var sample = ldmdb.Sample.Find(image.SampleID);
                sample.ImageCount -= 1;

                ldmdb.Image.Remove(image);
                await ldmdb.SaveChangesAsync();

                return NoContent();
            }
        }

        // GET:
        [HttpGet("{dataset_id}/{sample_id}/img={id}")]
        public async Task<Object> GetImageView(string dataset_id, int sample_id, string id)
        {
            using(var ldmdb = new LDMContext(dataset_id))
            {
                var image = await ldmdb.Image.Where(i => (i.SampleID == sample_id && i.ImageID == id)).FirstAsync();
                if (image == null)
                {
                    return NotFound();
                }



                return NoContent();
            }
        }

        private bool ImageExists(LDMContext ldmdb, string id)
        {
            return ldmdb.Image.Any(e => e.ImageID == id);
        }

        private Object ImageToResponseType(Image img)
        {
            return new
            {
                img.ImageID,
                img.SampleID,
                img.ImageNO,
                img.ImageCode,
                img.OriginalFilename,
                img.ImageScheme,
                Sample = new
                {
                    img.Sample.SampleID,
                    img.Sample.DatasetID,
                    img.Sample.SampleType,
                    img.Sample.Metadata,
                    img.Sample.ImageCount
                }
            };
        }
    }
}
