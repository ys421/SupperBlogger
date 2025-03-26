using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggerLibrary;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Article> Articles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}