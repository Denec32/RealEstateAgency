using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;
using Refit;

namespace RealEstateAgency
{
    public interface IBuilding
    {
        [Get("/api/Building")]
        Task<IEnumerable<Building>> GetBuildings();

        [Get("/api/Building/{id}")]
        Task<Building> GetBuilding(int id);

        [Put("/api/Building")]
        Task<Building> UpdateBuilding([Body] Building building);

        [Post("/api/Building")]
        Task<Building> PostBuilding([Body] Building building);        
        
        [Delete("/api/Building/{id}")]
        Task<Building> DeleteBuilding(int id);
    }
}
