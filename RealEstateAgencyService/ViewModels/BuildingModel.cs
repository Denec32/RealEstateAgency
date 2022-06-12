using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.ViewModels
{
    public class BuildingModel
    {
        public Building? Building { get; set; }

        public List<ListingModel>? Listings { get; set; }
    }
}
