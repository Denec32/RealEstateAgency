using System.ComponentModel.DataAnnotations;

namespace RealEstateAgencyService.Models
{
    public class RealEstatePhoto
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Photo { get; set; }

        public Listing? Listing { get; set; }
        public int ListingId { get; set; }
    }
}
