using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.ViewModels
{
    public class ListingModel
    {
        public Listing? Listing { get; set; }

        public User? User { get; set; }

        public ListingStatus? ListingStatus { get; set; }

        public List<RealEstatePhoto>? RealEstatePhotos { get; set; }
    }
}
