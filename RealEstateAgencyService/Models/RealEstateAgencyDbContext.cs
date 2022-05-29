using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateAgencyService.Data;
using Microsoft.Extensions;

namespace RealEstateAgencyService.Models
{
    public class RealEstateAgencyDbContext: IdentityDbContext<User>
    {
        protected readonly IConfiguration Configuration;

        public RealEstateAgencyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Listing>? Listings { get; set; }
        public DbSet<ListingStatus>? ListingStatuses { get; set; }
        public DbSet<RealEstatePhoto>? RealEstatePhotos { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Building>? Buildings { get; set; }
        public DbSet<BuildingPhoto>? BuildingPhotos { get; set; }
        public DbSet<Favourite>? Favourites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=DENECLAPTOP\\SQLEXPRESS;Database=RealEstateAgency;Trusted_Connection=True;MultipleActiveResultSets=true");
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddData();
        }
    }
}
