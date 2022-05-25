using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        RealEstateAgencyDbContext db;
        private readonly UserManager<User> _userManager;
        public UserController(RealEstateAgencyDbContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {

            return await db.Users.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var ls = await db.Listings.ToListAsync();
            var selected = ls.Where(x => x.UserId == user.Id).ToList();
            user.Listings = selected;
            var photos = (from t1 in db.RealEstatePhotos select t1).ToList();
            foreach (var item in user.Listings)
            {
                item.RealEstatePhotos = photos.Where(x => x.ListingId == item.Id).ToList();
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put([FromBody] User model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.PhoneNumber = model.PhoneNumber;
            user.LastName = model.LastName;
            user.FirstName = model.FirstName;

            await _userManager.UpdateAsync(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
