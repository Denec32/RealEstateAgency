using Refit;
using RealEstateAgencyService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAgency
{
    public interface IRealEstateAgencyServiceAPI
    {
        [Get("/api/ListingModel")]
        Task <IEnumerable<ListingModel>> GetListingModel();      
        
        [Get("/api/ListingModel/{id}")]
        Task <ListingModel> GetListingModel(int id);   
        
        [Get("/api/Listing/{id}")]
        Task <Listing> GetListing(int id);

        [Get("/api/ListingStatus")]
        Task<IEnumerable<ListingStatus>> GetListingStatuses();       
        
        [Post("/api/Listing")]
        Task<Listing> PostListing([Body] Listing model);        
        
        [Put("/api/Listing")] 
        Task<Listing> UpdateListing([Body] Listing model);  

        [Delete("/api/Listing/{id}")]
        Task DeleteListing(int id);
    }
}
