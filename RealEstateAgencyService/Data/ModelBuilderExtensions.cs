using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Data
{
    public static class ModelBuilderExtensions
    {
        public static void AddData(this ModelBuilder mb)
        {
            
            mb.Entity<ListingStatus>().HasData(
                    new ListingStatus
                    {
                        Id = 1,
                        Status = "Активен",
                    },
                    new ListingStatus
                    {
                        Id = 2,
                        Status = "Неактивен",
                    });    
        }
    }
}
