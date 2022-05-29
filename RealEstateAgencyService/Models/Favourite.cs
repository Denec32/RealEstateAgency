using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgencyService.Models
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ListingId { get; set; }
    }
}
