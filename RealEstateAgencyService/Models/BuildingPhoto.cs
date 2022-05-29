using System.ComponentModel.DataAnnotations;

namespace RealEstateAgencyService.Models
{
    public class BuildingPhoto
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Photo { get; set; }

        public Building? Building { get; set; }
        public int BuildingId { get; set; }
    }
}
