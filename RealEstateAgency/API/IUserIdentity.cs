using RealEstateAgencyService.Models;
using RealEstateAgencyService.ViewModels;
using Refit;

namespace RealEstateAgency
{
    public interface IUserIdentityAPI
    {

        [Get("/api/User/{id}")]
        Task<User> GetUser(string id);

        [Put("/api/User")]
        Task<User> UpdateUser([Body] User model);

        [Post("/api/Account")]
        Task<RegisterViewModel> RegisterUser([Body] RegisterViewModel model);

        [Get("/api/Phone/{id}")]
        Task<string> GetPhoneNumber(string id);

        [Get("/api/Username/{id}")]
        Task<string> GetFirstName(string id);
    }
}
