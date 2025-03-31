using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BloggerBlazorServer.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAndContributorAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // 1) 기본 역할 생성: Admin, Contributor, User
            string[] roleNames = { "Admin", "Contributor", "User" };
            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2) 관리자 계정 생성 (a@a.a)
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
                    IsActive = true // ★ 관리자 계정은 기본적으로 활성 상태
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    // DB 반영
                    await userManager.UpdateAsync(adminUser);
                }
                }
                else
                {
                // 이미 존재한다면, 관리자 계정이 비활성 상태일 수도 있으므로 활성으로 강제
                adminUser.IsActive = true; // ★ 관리자 계정은 항상 활성
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                // DB 반영
                await userManager.UpdateAsync(adminUser);
            }


            // 3) 컨트리뷰터 계정 생성 (c@c.c) – 기본 비활성 상태
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
                    IsActive = false // 기본적으로 비활성 상태로 설정
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
                // 이미 존재하는 경우에도 비활성 상태로 강제
                contributorUser.IsActive = false;
                if (!await userManager.IsInRoleAsync(contributorUser, "Contributor"))
                {
                    await userManager.AddToRoleAsync(contributorUser, "Contributor");
                }
                await userManager.UpdateAsync(contributorUser);
            }

        }
    }
}
