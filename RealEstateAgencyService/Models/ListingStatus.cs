using System.ComponentModel.DataAnnotations;

namespace RealEstateAgencyService.Models
{
    public class ListingStatus
    {
        [Key]
        public int Id { get; set; }
        public string? Status { get; set; }

        public List<Listing>? Listings { get; set; }
    }
}
