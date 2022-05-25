using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgencyService.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        public int Rooms { get; set; }

        public int Space { get; set; }

        public int Floor { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public DateTime Created { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? Street { get; set; }

        public string? Building { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public ListingStatus? ListingStatus { get; set; }
        public int ListingStatusId { get; set; }
        //[JsonIgnore]
        public List<RealEstatePhoto>? RealEstatePhotos { get; set; }
    }
}
