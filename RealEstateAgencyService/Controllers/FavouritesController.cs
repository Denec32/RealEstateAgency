using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public FavouritesController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

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
    }
}
