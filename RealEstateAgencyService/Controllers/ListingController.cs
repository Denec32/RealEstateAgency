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

        //GET api/listing
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> Get()
        {
            if (db.Listings == null)
            {
                return BadRequest();
            }

            var listings = await db.Listings.ToListAsync();
            var photos = (from t1 in db.RealEstatePhotos select t1).ToList();

            if (photos == null)
            {
                return Ok(listings);
            }

            foreach (var listing in listings)
            {
                listing.RealEstatePhotos = photos.Where(x => x.ListingId == listing.Id).ToList();
            }

            return Ok(listings);
        }

        //GET api/listing/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Listing>>> Get(int id)
        {
            if (db.Listings == null)
            {
                return BadRequest();
            }

            var listing = await db.Listings.FirstOrDefaultAsync(x => x.Id == id);

            if (listing == null)
            {
                return NotFound();
            }

            var photos = (from t1 in db.RealEstatePhotos select t1).ToList();

            if (photos == null)
            {
                return Ok(listing);
            }

            listing.RealEstatePhotos = photos.Where(x => x.ListingId == listing.Id).ToList();

            return Ok(listing);
        }

        //POST api/listing
        [HttpPost]
        public async Task<ActionResult<Listing>> Post(Listing listing)
        {
            if (listing == null)
            {
                return BadRequest();
            }
            db.Listings?.Add(listing);

            if (listing.RealEstatePhotos != null)
            {
                foreach (var photo in listing.RealEstatePhotos)
                {
                    if (photo.ListingId != listing.Id)
                    {
                        photo.ListingId = listing.Id;
                    }
                    db.RealEstatePhotos?.Add(photo);
                }
            }

            await db.SaveChangesAsync();
            return Ok(listing);
        }

        //PUT api/listing
        [HttpPut]
        public async Task<ActionResult<Listing>> Put([FromBody] Listing listing)
        {
            if (listing == null)
            {
                return BadRequest();
            }

            if (db.Listings == null || !db.Listings.Any(x => x.Id == listing.Id))
            {
                return NotFound();
            }

            db.Update(listing);
            await db.SaveChangesAsync();
            return Ok(listing);
        }

        //DELETE api/listing/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Listing>> Delete(int id)
        {
            if (db.Listings == null)
            {
                return BadRequest();
            }

            var listing = await db.Listings.FirstOrDefaultAsync(x => x.Id == id);

            if (listing == null)
            {
                return NotFound();
            }

            db.Listings.Remove(listing);
            await db.SaveChangesAsync();
            return Ok(listing);
        }
    }
}
