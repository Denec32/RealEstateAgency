using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.ViewModels
{
    public class FavouriteViewModel
    {
        public User User { get; set; }
        public List<Listing> Listings { get; set; }
    }
}
