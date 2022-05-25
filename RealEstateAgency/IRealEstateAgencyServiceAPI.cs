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

        [Get("/api/phone/{id}")]
        Task<string> GetPhoneNumber(string id);           
        
        [Get("/api/username/{id}")]
        Task<string> GetFirstName(string id);            
        
        [Get("/api/user/{id}")]
        Task<User> GetUser(string id);    

        [Post("/api/Account")]
        Task<RegisterViewModel> RegisterUser([Body] RegisterViewModel model);     
        
        [Post("/api/Listing")]
        Task<Listing> PostListing([Body] Listing model);        
        
        [Put("/api/Listing")] 
        Task<Listing> UpdateListing([Body] Listing model);  
        
        [Put("/api/User")]
        Task<User> UpdateUser([Body] User model);

        [Delete("/api/Listing/{id}")]
        Task DeleteListing(int id);
    }
}
