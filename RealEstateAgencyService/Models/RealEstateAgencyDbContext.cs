using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Data;

namespace RealEstateAgencyService.Models
{
    public class RealEstateAgencyDbContext: IdentityDbContext<User>
    {
        public RealEstateAgencyDbContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Listing>? Listings { get; set; }
        public DbSet<ListingStatus>? ListingStatuses { get; set; }
        public DbSet<RealEstatePhoto>? RealEstatePhotos { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=DENECLAPTOP\\SQLEXPRESS;Database=RealEstateAgency;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddData();
        }
    }
}
