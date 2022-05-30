using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateAgency.Controllers
{
    [Authorize(Roles = "admin")]
    public class BuildingsController : Controller
    {
        private readonly IBuilding _service;

        public BuildingsController(IBuilding service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var buildings = _service.GetBuildings().Result;
            return View(buildings);
        }
    }
}
