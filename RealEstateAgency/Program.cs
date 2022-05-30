using RealEstateAgency;
using Refit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using RealEstateAgencyService.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RealEstateAgencyDbContext>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<RealEstateAgencyDbContext>();

builder.Services.AddControllersWithViews();

var settings = new RefitSettings()
{
    ContentSerializer = new NewtonsoftJsonContentSerializer(),
    Buffered = true
};

builder.Services.AddRefitClient<IRealEstateAgencyServiceAPI>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7007"));

builder.Services.AddRefitClient<IUserIdentityAPI>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7007"));

builder.Services.AddRefitClient<IBuilding>(settings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7007"));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling =
    ReferenceLoopHandling.Ignore);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
