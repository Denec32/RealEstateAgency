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
        private readonly IRealEstateAgencyServiceAPI _service;
        public HomeController(IRealEstateAgencyServiceAPI api)
        {
            _service = api;
        }

        public IActionResult Index()
        {
            List<ListingModel> lm = _service.GetListingModel().Result.ToList();
            List<ListingModel> selected = lm.Where(x => x.ListingStatus.Id == 1).ToList();
            return View(selected);
        }
        public IActionResult Listing(int id)
        {
            ListingModel lm = _service.GetListingModel(id).Result;

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
                Listing listing = _service.GetListing(id).Result;
                List<ListingStatus> statuses = _service.GetListingStatuses().Result.ToList();

                if (listing.ListingStatusId == statuses[0].Id)
                {
                    listing.ListingStatusId = statuses[1].Id;
                }
                else
                {
                    listing.ListingStatusId = statuses[0].Id;
                }

                await _service.UpdateListing(listing);
            }
            return RedirectToAction("Index", "Account");
        }
        public async Task<IActionResult> RemoveListingAsync(int id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                await _service.DeleteListing(id);
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
            model.Listing.ListingStatusId = 1;

            model.Listing.BuildingId = 1;

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

            await _service.PostListing(model.Listing);

            return View(model);
        }

        public IActionResult UpdateListing(int id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                var list = _service.GetListing(id).Result;
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

            _service.UpdateListing(listing);

            return View(listing);
        }
    }
}