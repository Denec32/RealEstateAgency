using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Models;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace RealEstateAgency.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRealEstateAgencyServiceAPI _listingService;
        private readonly IUserIdentityAPI _userService;
        private readonly IBuilding _buildingService;
        public HomeController(IRealEstateAgencyServiceAPI listingService, IUserIdentityAPI userService, IBuilding buildingService)
        {
            _listingService = listingService;
            _userService = userService;
            _buildingService = buildingService;
        }

        public IActionResult Index()
        {
            List<ListingModel> lm = _listingService.GetListingModel().Result.ToList();
            List<ListingModel> selected = lm.Where(x => x.ListingStatus.Id == 1).ToList();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Favourite> favs = _userService.GetFavourites(userId).Result.ToList();

            // Check if listing is in favourites
            foreach (var favouriteListing in favs)
            {
                for (int i = 0; i < selected.Count; i++)
                {
                    if (selected[i].Listing != null && selected[i].Listing.Id == favouriteListing.ListingId)
                    {
                        selected[i].IsFavourite = true;
                    }
                }
            }

            // Check if user owns a listing
            foreach (var item in selected)
            {
                if (userId == item.User.Id)
                {
                    item.IsOwned = true;
                }
            }

            return View(selected);
        }
        public IActionResult Listing(int id)
        {
            ListingModel lm = _listingService.GetListingModel(id).Result;

            return View(lm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateListing()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangeVisabilityAsync(int id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                Listing listing = _listingService.GetListing(id).Result;
                List<ListingStatus> statuses = _listingService.GetListingStatuses().Result.ToList();

                if (listing.ListingStatusId == statuses[0].Id)
                {
                    listing.ListingStatusId = statuses[1].Id;
                }
                else
                {
                    listing.ListingStatusId = statuses[0].Id;
                }

                await _listingService.UpdateListing(listing);
            }
            return RedirectToAction("Index", "Account");
        }
        public async Task<IActionResult> RemoveListingAsync(int id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                await _listingService.DeleteListing(id);
            }
            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> PostListingAsync(ListAndPhotosViewModel model)
        {
            if (model.Listing is null)
            {
                return RedirectToAction("Error", "Account");
            }

            model.Listing.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.Listing.Created = DateTime.Now;

            var status = _listingService.GetListingStatuses().Result.ToList().FirstOrDefault(x => x.Status == "Активен");
            if (status is null)
            {
                return RedirectToAction("Error", "Account");
            }
            model.Listing.ListingStatusId = status.Id;

            var building = _buildingService.GetBuildings().Result.FirstOrDefault(x => x.Region == model.Listing.Region && x.City == model.Listing.City
            && x.BuildingNumber == model.Listing.BuildingNumber && x.Street == model.Listing.Street);

            if (building is null)
            {
                building = new Building
                {
                    Region = model.Listing.Region,
                    City = model.Listing.City,
                    Street = model.Listing.Street,
                    BuildingNumber = model.Listing.BuildingNumber,
                };
                await _buildingService.PostBuilding(building);

                building = _buildingService.GetBuildings().Result.FirstOrDefault(x => x.Region == model.Listing.Region && x.City == model.Listing.City
                && x.BuildingNumber == model.Listing.BuildingNumber && x.Street == model.Listing.Street);
            }

            model.Listing.BuildingId = building.Id;

            model.Listing.RealEstatePhotos = new List<RealEstatePhoto>();
            foreach (var item in model.Photos)
            {
                byte[] imageData;
                using (var binaryReader = new BinaryReader(item.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)item.Length);
                }

                var photo = new RealEstatePhoto
                {
                    ListingId = model.Listing.Id,
                    Photo = imageData
                };

                model.Listing.RealEstatePhotos.Add(photo);
            }

            await _listingService.PostListing(model.Listing);

            return View(model);
        }

        public IActionResult UpdateListing(int id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                var list = _listingService.GetListing(id).Result;
                return View(list);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult PutListing(Listing listing)
        {
            listing.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            listing.Created = DateTime.Now;
            listing.ListingStatusId = 1;

            _listingService.UpdateListing(listing);

            return View(listing);
        }
    }
}