using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingStatusController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public ListingStatusController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingStatus>>> Get()
        {

            return await db.ListingStatuses.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListingStatus>> Get(int id)
        {
            ListingStatus list = await db.ListingStatuses.FirstOrDefaultAsync(x => x.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }

        [HttpPost]
        public async Task<ActionResult<ListingStatus>> Post(ListingStatus status)
        {
            if (status == null)
            {
                return BadRequest();
            }

            db.ListingStatuses.Add(status);
            await db.SaveChangesAsync();
            return Ok(status);
        }

        [HttpPut]
        public async Task<ActionResult<ListingStatus>> Put(ListingStatus status)
        {
            if (status == null)
            {
                return BadRequest();
            }
            if (!db.ListingStatuses.Any(x => x.Id == status.Id))
            {
                return NotFound();
            }

            db.Update(status);
            await db.SaveChangesAsync();
            return Ok(status);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ListingStatus>> Delete(int id)
        {
            ListingStatus status = db.ListingStatuses.FirstOrDefault(x => x.Id == id);
            if (status == null)
            {
                return NotFound();
            }
            db.ListingStatuses.Remove(status);
            await db.SaveChangesAsync();
            return Ok(status);
        }
    }
}
