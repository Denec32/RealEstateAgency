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


        private readonly ILogger<HomeController> _logger;
        private readonly IRealEstateAgencyServiceAPI _serviceAPI;
        public HomeController(ILogger<HomeController> logger,
            IRealEstateAgencyServiceAPI api)
        {
            _logger = logger;
            _serviceAPI = api;
        }

        public IActionResult Index()
        {
            List<ListingModel> lm = _serviceAPI.GetListingModel().Result.ToList();
            List<ListingModel> selected = lm.Where(x => x.ListingStatus.Id == 1).ToList();
            return View(selected);
        }
        public IActionResult Listing(int id)
        {
            ListingModel lm = _serviceAPI.GetListingModel(id).Result;

            return View(lm);
        }

        public string GetPhone(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string phoneNum = _serviceAPI.GetPhoneNumber(id).Result;
                return phoneNum;
            }
            else
            {
                return string.Empty;
            }
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
            if (User.Identity.IsAuthenticated)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ChangeVisabilityAsync(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Listing listing = _serviceAPI.GetListing(id).Result;
                List<ListingStatus> statuses = _serviceAPI.GetListingStatuses().Result.ToList();

                if (listing.ListingStatusId == statuses[0].Id)
                {
                    listing.ListingStatusId = statuses[1].Id;
                }
                else
                {
                    listing.ListingStatusId = statuses[0].Id;
                }

                await _serviceAPI.UpdateListing(listing);
            }
            return RedirectToAction("Index", "Account");
        }
        public async Task<IActionResult> RemoveListingAsync(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _serviceAPI.DeleteListing(id);
            }
            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> PostListingAsync(ListAndPhotosViewModel l)
        {
            l.Listing.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            l.Listing.Created = DateTime.Now;
            l.Listing.ListingStatusId = 1;

            byte[] imageData = null;
            l.Listing.RealEstatePhotos = new List<RealEstatePhoto>();
            foreach (var item in l.Photos)
            {
                using (var binaryReader = new BinaryReader(item.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)item.Length);
                }

                var photo = new RealEstatePhoto
                {
                    ListingId = l.Listing.Id,
                    Photo = imageData
                };

                l.Listing.RealEstatePhotos.Add(photo);
            }


            await _serviceAPI.PostListing(l.Listing);

            return View(l);
        }

        public IActionResult UpdateListing(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var list = _serviceAPI.GetListing(id).Result;
                return View(list);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult PutListing(Listing l)
        {
            l.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            l.Created = DateTime.Now;
            l.ListingStatusId = 1;

            _serviceAPI.UpdateListing(l);

            return View(l);
        }
    }
}