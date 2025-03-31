using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BloggerBlazorServer.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var designTimeConnectionString = "Server=localhost;Database=blogdb;User Id=sa;Password=YourPass;";
            builder.UseSqlServer(designTimeConnectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
