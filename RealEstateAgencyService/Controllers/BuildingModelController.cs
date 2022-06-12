using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;

namespace RealEstateAgencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingModelController : ControllerBase
    {
        RealEstateAgencyDbContext db;

        public BuildingModelController(RealEstateAgencyDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingModel>>> Get()
        {
            var buildings = await db.Buildings.ToListAsync();

            var listings = new List<ListingModel>();
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
                listings.Add(listingModel);
            }

            var buildingModels = new List<BuildingModel>();

            foreach (var item in buildings)
            {
                var buildingModel = new BuildingModel
                {
                    Building = item,
                    Listings = listings.Where(x => x.Listing.Region == item.Region && x.Listing.City == item.City
                    && x.Listing.Street == item.Street && x.Listing.BuildingNumber == item.BuildingNumber).ToList()
                };
                if (buildingModel.Listings.Count != 0)
                {
                    buildingModels.Add(buildingModel);
                }
            }

            return buildingModels;
            
        }
    }
}
