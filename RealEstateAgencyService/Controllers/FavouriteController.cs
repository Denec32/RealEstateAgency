using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public FavouriteController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        //GET api/favourite/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Favourite>> Get(string id)
        {
            if (db.Favourites == null)
            {
                return BadRequest();
            }
            var query = await db.Favourites.ToListAsync();
            var favourites = query.Where(x => x.UserId == id);
            return Ok(favourites);
        }

        //POST api/favourite
        [HttpPost]
        public async Task<ActionResult<Favourite>> Post(FavouritePostModel fvm)
        {
            if (fvm == null || fvm.UserId == null)
            {
                return BadRequest();
            }


            Favourite favourite = new Favourite
            {
                UserId = fvm.UserId,
                ListingId = fvm.ListingId
            };

            db.Favourites?.Add(favourite);

            await db.SaveChangesAsync();
            return Ok(fvm);
        }

        //DELETE api/favourite/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Favourite>> Delete(int id)
        {
            if (db.Favourites == null)
            {
                return BadRequest();
            }

            var fav = await db.Favourites.FirstOrDefaultAsync(x => x.Id == id);

            if (fav == null)
            {
                return NotFound();
            }

            db.Favourites.Remove(fav);
            await db.SaveChangesAsync();
            return Ok(fav);
        }
    }
}
