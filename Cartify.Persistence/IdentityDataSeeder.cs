using Cartify.Domain.Entities;
using Cartify.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Cartify.Persistence;

public class IdentityDataSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    : IIdentityDataSeeder
{
    public async Task SeedRolesAsync()
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new AppRole { Id = Guid.NewGuid().ToString(), Name = "Admin" });
            await roleManager.CreateAsync(new AppRole { Id = Guid.NewGuid().ToString(), Name = "Customer" });
        }
    }

    public async Task SeedUsersAsync()
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                Name = "Abdelrahman Admin",
                UserName = "admin@cartify.com",
                Email = "admin@cartify.com",
                RefreshToken = string.Empty
            };

            var customer = new AppUser
            {
                Name = "Abdelrahman Customer",
                UserName = "customer@cartify.com",
                Email = "customer@cartify.com",
                RefreshToken = string.Empty
            };
            await userManager.CreateAsync(user, "P@ssw0rd");
            await userManager.AddToRoleAsync(user, "Admin");

            await userManager.CreateAsync(customer, "P@ssw0rd");
            await userManager.AddToRoleAsync(customer, "Customer");
        }
    }
}