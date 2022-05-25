using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public ListingController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> Get()
        {

            return await db.Listings.ToListAsync();

        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Listing>> Get(int id)
        {
            Listing list = await db.Listings.FirstOrDefaultAsync(x => x.Id== id);
            var photos = (from t1 in db.RealEstatePhotos select t1).ToList();
            list.RealEstatePhotos = photos.Where(x => x.ListingId == list.Id).ToList();
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Listing>> Post(Listing list)
        {
            if (list == null)
            {
                return BadRequest();
            }
            db.Listings.Add(list);
            //await db.SaveChangesAsync();

            foreach (var item in list.RealEstatePhotos)
            {
                //item.ListingId = list.Id;
                db.RealEstatePhotos.Add(item);
            }

            await db.SaveChangesAsync();
            return Ok(list);
        }

        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<Listing>> Put([FromBody] Listing list)
        {
            if (list == null)
            {
                return BadRequest();
            }
            if (!db.Listings.Any(x => x.Id == list.Id))
            {
                return NotFound();
            }

            db.Update(list);
            await db.SaveChangesAsync();
            return Ok(list);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Listing>> Delete(int id)
        {
            Listing list = db.Listings.FirstOrDefault(x => x.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            db.Listings.Remove(list);
            await db.SaveChangesAsync();
            return Ok(list);
        }
    }
}
