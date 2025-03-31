using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using BloggerBlazorServer.Models;

namespace BloggerBlazorServer.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAndContributorAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            string[] roleNames = { "Admin", "Contributor", "User" };
            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "a@a.a";
            var adminPassword = "P@$$w0rd";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.UpdateAsync(adminUser);
                }
                }
                else
                {
                adminUser.IsActive = true;
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                await userManager.UpdateAsync(adminUser);
            }

            var contributorEmail = "c@c.c";
            var contributorPassword = "P@$$w0rd";
            var contributorUser = await userManager.FindByEmailAsync(contributorEmail);
            if (contributorUser == null)
            {
                contributorUser = new ApplicationUser
                {
                    UserName = contributorEmail,
                    Email = contributorEmail,
                    EmailConfirmed = true,
                    IsActive = false
                };

                var result = await userManager.CreateAsync(contributorUser, contributorPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(contributorUser, "Contributor");
                    await userManager.UpdateAsync(contributorUser);
                }
            }
            else
            {
                contributorUser.IsActive = false;
                if (!await userManager.IsInRoleAsync(contributorUser, "Contributor"))
                {
                    await userManager.AddToRoleAsync(contributorUser, "Contributor");
                }
                await userManager.UpdateAsync(contributorUser);
            }
            
            if (!await dbContext.Articles.AnyAsync())
            {
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin != null)
                {
                    var articles = new[]
                    {
                        new Article
                        {
                            Title = "What is SAN??",
                            Body = " 'SAN'is blog name which stands for our blog developer names. S for Seungyeoop, A for Alex, and N for Nischey. Welcome to our blog and thank you for viewing our article.",
                            StartDate = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                            EndDate = new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                            IsPublished = true,
                            CreatedAt = new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = admin.Id,
                            LastModifiedBy = admin.Id
                        }
                    };

                    dbContext.Articles.AddRange(articles);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
