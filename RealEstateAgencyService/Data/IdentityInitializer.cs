using Microsoft.AspNetCore.Identity;
using RealEstateAgencyService.Models;

namespace RealEstateAgencyService.Data
{
    public class IdentityInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string userEmail = "dzenevich@mail.ru";
            string password = "_Aa123456";
            string firstName = "Денис";
            string lastName = "Зеневич";
            string phone = "89206798889";

            await roleManager.CreateAsync(new IdentityRole("user"));
            await roleManager.CreateAsync(new IdentityRole("admin"));

            User user = new User { Email = userEmail, UserName = userEmail, FirstName = firstName, LastName = lastName, PhoneNumber = phone };
            IdentityResult result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }

            userEmail = "pzubkov@mail.ru";
            firstName = "Павел";
            lastName = "Зубков";
            phone = "89204633759";

            user = new User { Email = userEmail, UserName = userEmail, FirstName = firstName, LastName = lastName, PhoneNumber = phone };
            result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");
            }            
            
            userEmail = "eremindan@mail.ru";
            firstName = "Даниил";
            lastName = "Еремин";
            phone = "89206731373";

            user = new User { Email = userEmail, UserName = userEmail, FirstName = firstName, LastName = lastName, PhoneNumber = phone };
            result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");
            }
        }
    }
}
