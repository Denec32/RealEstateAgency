using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;
using Refit;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace RealEstateAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserIdentityAPI _userService;
        private readonly IRealEstateAgencyServiceAPI _listingService;
        private readonly SignInManager<User> _signInManager;
        public AccountController(IUserIdentityAPI userApi, IRealEstateAgencyServiceAPI listingApi, SignInManager<User> signInManager)
        {
            _userService = userApi;
            _listingService = listingApi;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var postModel = new RegisterViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    PasswordConfirm = model.PasswordConfirm,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName             
                };
                var user = new User {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                try
                { 
                    var request = await _userService.RegisterUser(postModel);
                }
                catch (ApiException ex)
                {
                    Debug.WriteLine(ex.Message);
                    return RedirectToAction("Register", "Account");
                }

                await _userService.RegisterUser(postModel);

                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        public IActionResult Favourites()
        {

            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = _userService.GetUser(userId).Result;

                List<Listing> listings = _listingService.GetListing().Result.ToList();

                var favs = _userService.GetFavourites(userId).Result.ToList();

                var ids = new List<int>();

                foreach (var item in favs)
                {
                    ids.Add(item.ListingId);
                }

                var selected = listings.Where(l => ids.Contains(l.Id)).ToList();
                var fpm = new FavouriteViewModel { 
                    User = user,
                    Listings = selected
                };
                return View(fpm);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }

        public IActionResult Index()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user =  _userService.GetUser(userId).Result;
                return View(user);
            }
            return View();
        }

        public string GetFirstName(string id)
        {
            string name = "";
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                try
                {
                    name = _userService.GetFirstName(id).Result;
                }
                catch (ApiException ex)
                {
                    Debug.WriteLine(ex.Message);
                    _signInManager.SignOutAsync();
                    return "No User Found";
                }
            }
            return name;
        }

        public async Task<IActionResult> UpdateUserAsync(User user)
        {
            
            await _userService.UpdateUser(user);
            return RedirectToAction("Index", "Account");
        }

        public string GetPhone(string id)
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                string phoneNum = _userService.GetPhoneNumber(id).Result;
                return phoneNum;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<bool> PostFavourite(int id)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userListing = await _userService.GetUser(currentUserId);
            if (userListing.Listings is not null)
            {
                foreach (var listing in userListing.Listings)
                {
                    if (listing.Id == id)
                    {
                        return false;
                    }
                }
            }

            var fm = new FavouritePostModel
            {
                ListingId = id,
                UserId = currentUserId
            }; 

            await _userService.PostFavourite(fm);
            return true;
        }
    }
}
