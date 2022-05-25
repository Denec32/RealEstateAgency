using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstatePhotoController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public RealEstatePhotoController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RealEstatePhoto>>> Get()
        {

            return await db.RealEstatePhotos.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RealEstatePhoto>> Get(int id)
        {
            RealEstatePhoto photo = await db.RealEstatePhotos.FirstOrDefaultAsync(x => x.Id == id);
            if (photo == null)
            {
                return NotFound();
            }
            return new ObjectResult(photo);
        }

        [HttpPost]
        public async Task<ActionResult<RealEstatePhoto>> Post(RealEstatePhoto photo)
        {
            if (photo == null)
            {
                return BadRequest();
            }

            db.RealEstatePhotos.Add(photo);
            await db.SaveChangesAsync();
            return Ok(photo);
        }

        [HttpPut]
        public async Task<ActionResult<RealEstatePhoto>> Put(RealEstatePhoto photo)
        {
            if (photo == null)
            {
                return BadRequest();
            }
            if (!db.RealEstatePhotos.Any(x => x.Id == photo.Id))
            {
                return NotFound();
            }

            db.Update(photo);
            await db.SaveChangesAsync();
            return Ok(photo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RealEstatePhoto>> Delete(int id)
        {
            RealEstatePhoto photo = db.RealEstatePhotos.FirstOrDefault(x => x.Id == id);
            if (photo == null)
            {
                return NotFound();
            }
            db.RealEstatePhotos.Remove(photo);
            await db.SaveChangesAsync();
            return Ok(photo);
        }
    }
}
