using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingModelController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public ListingModelController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingModel>>> Get()
        {
            var list = new List<ListingModel>();
            var photos = (from t1 in db.RealEstatePhotos select t1).ToList();

            var listingList = (from t1 in db.Listings select t1).ToList();

            foreach (var item in listingList)
            {
                var listingModel = new ListingModel
                {
                    Listing = item,
                    User = await db.Users.FirstOrDefaultAsync(x => x.Id == item.UserId),
                    ListingStatus = await db.ListingStatuses.FirstOrDefaultAsync(x => x.Id == item.ListingStatusId),

                    RealEstatePhotos = photos.Where(x => x.ListingId == item.Id).ToList()
                };
                list.Add(listingModel);
            }
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListingModel>> Get(int id)
        {

            Listing listing = await db.Listings.FirstOrDefaultAsync(x => x.Id == id);

            var listingModel = new ListingModel
            {
                Listing = listing,
                User = await db.Users.FirstOrDefaultAsync(x => x.Id == listing.UserId),
                ListingStatus = await db.ListingStatuses.FirstOrDefaultAsync(x => x.Id == listing.ListingStatusId),

                RealEstatePhotos = (from t1 in db.RealEstatePhotos select t1).ToList().Where(x => x.ListingId == listing.Id).ToList()
            };
            return listingModel;
        }
    }
}
