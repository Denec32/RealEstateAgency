using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        RealEstateAgencyDbContext db;
        public BuildingController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        //GET api/building
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> Get()
        {
            if (db.Buildings == null)
            {
                return BadRequest();
            }

            var buildings = await db.Buildings.ToListAsync();
            var photos = (from t1 in db.BuildingPhotos select t1).ToList();

            if (photos == null)
            {
                return Ok(buildings);
            }

            foreach (var building in buildings)
            {
                building.BuildingPhotos = photos.Where(x => x.BuildingId == building.Id).ToList();
            }

            return Ok (buildings);
        }

        //GET api/building/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Building>>> Get(int id)
        {
            if (db.Buildings == null)
            {
                return BadRequest();
            }

            var building = await db.Buildings.FirstOrDefaultAsync(x => x.Id == id);

            if (building == null)
            {
                return NotFound();
            }

            var photos = (from t1 in db.BuildingPhotos select t1).ToList();

            if (photos == null)
            {
                return Ok(building);
            }

            building.BuildingPhotos = photos.Where(x => x.BuildingId == building.Id).ToList();

            return Ok(building);
        }

        //POST api/building
        [HttpPost]
        public async Task<ActionResult<Building>> Post(Building building)
        {
            if (building == null)
            {
                return BadRequest();
            }

            db.Buildings?.Add(building);

            if (building.BuildingPhotos != null)
            {
                foreach (var photo in building.BuildingPhotos)
                {
                    if (photo.BuildingId != building.Id)
                    {
                        photo.BuildingId = building.Id;
                    }
                    db.BuildingPhotos?.Add(photo);
                }
            }

            await db.SaveChangesAsync();
            return Ok(building);
        }

        //PUT api/building
        [HttpPut]
        public async Task<ActionResult<Listing>> Put([FromBody] Building building)
        {
            if (building == null)
            {
                return BadRequest();
            }

            if (db.Buildings == null || !db.Buildings.Any(x => x.Id == building.Id))
            {
                return NotFound();
            }

            db.Update(building);
            await db.SaveChangesAsync();
            return Ok(building);
        }


        //DELETE api/building/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Building>> Delete(int id)
        {
            if (db.Buildings == null)
            {
                return BadRequest();
            }

            var building = await db.Buildings.FirstOrDefaultAsync(x => x.Id == id);

            if (building == null)
            {
                return NotFound();
            }

            db.Buildings.Remove(building);
            await db.SaveChangesAsync();
            return Ok(building);
        }
    }
}
