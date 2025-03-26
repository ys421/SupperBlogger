using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BloggerBlazorServer.Components;
using BloggerBlazorServer.Components.Account;
using BloggerLibrary;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// 연결 문자열 설정
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__blogdb");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string 'blogdb' not found. Using default connection string.");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string not found.");
    }
}

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity 설정
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages();
app.MapAdditionalIdentityEndpoints();

// 데이터베이스 마이그레이션 및 시드 데이터
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    context.Database.Migrate();

    // 역할 생성
    if (!await roleManager.RoleExistsAsync(Roles.Admin))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }
    if (!await roleManager.RoleExistsAsync(Roles.Contributor))
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Contributor));
    }

    // 시드 데이터 추가
    var adminUser = await userManager.FindByEmailAsync("a@a.a");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "a@a.a",
            Email = "a@a.a",
            FirstName = "Admin",
            LastName = "User"
        };
        var result = await userManager.CreateAsync(adminUser, "P@$$w0rd");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }

    var contributorUser = await userManager.FindByEmailAsync("c@c.c");
    if (contributorUser == null)
    {
        contributorUser = new ApplicationUser
        {
            UserName = "c@c.c",
            Email = "c@c.c",
            FirstName = "Contributor",
            LastName = "User"
        };
        var result = await userManager.CreateAsync(contributorUser, "P@$$w0rd");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(contributorUser, Roles.Contributor);
        }
    }

    // 샘플 기사 데이터 추가
    if (!context.Articles.Any())
    {
        context.Articles.AddRange(
            new Article
            {
                Title = "First Article",
                Author = "a@a.a",
                Content = "This is the content of the first article.",
                PublishDate = DateTime.Now,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(30),
                IsPublished = true
            },
            new Article
            {
                Title = "Second Article",
                Author = "c@c.c",
                Content = "This is the content of the second article.",
                PublishDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(30),
                IsPublished = true
            },
            new Article
            {
                Title = "Draft Article",
                Author = "c@c.c",
                Content = "This is a draft article.",
                PublishDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                IsPublished = false
            }
        );
        context.SaveChanges();
    }
}

app.Run();

public static class Roles
{
    public const string Admin = "Admin";
    public const string Contributor = "Contributor";
}