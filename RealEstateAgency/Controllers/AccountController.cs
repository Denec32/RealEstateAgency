using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;
using Refit;
using System.Diagnostics;
using System.Security.Claims;

namespace RealEstateAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRealEstateAgencyServiceAPI _serviceAPI;
        private readonly SignInManager<User> _signInManager;
        public AccountController(ILogger<HomeController> logger,
            IRealEstateAgencyServiceAPI api, SignInManager<User> signInManager)
        {
            _logger = logger;
            _serviceAPI = api;
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
                    var request = await _serviceAPI.RegisterUser(postModel);
                }
                catch (ApiException ex)
                {
                    var content = ex.GetContentAsAsync<Dictionary<string, string>>();
                    Debug.WriteLine(ex.Message);
                    return RedirectToAction("Register", "Account");
                }

                await _serviceAPI.RegisterUser(postModel);

                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }
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
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user =  _serviceAPI.GetUser(userId).Result;
                return View(user);
            }
            return View();
        }

        public string GetFirstName(string id)
        {
            string name = "";
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    name = _serviceAPI.GetFirstName(id).Result;
                }
                catch (ApiException ex)
                {
                    var content = ex.GetContentAsAsync<Dictionary<string, string>>();
                    Debug.WriteLine(ex.Message);
                    _signInManager.SignOutAsync();
                    return "No User Found";
                }
            }
            return name;
        }

        public async Task<IActionResult> UpdateUserAsync(User user)
        {
            
            await _serviceAPI.UpdateUser(user);
            return RedirectToAction("Index", "Account");
        }
    }
}
