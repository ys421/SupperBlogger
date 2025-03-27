using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BloggerBlazorServer.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // 디자인 타임(마이그레이션)에서만 사용할 임시 연결 문자열
            // macOS/Linux라면 Windows 인증 대신 SQL 로그인 계정 사용
            var designTimeConnectionString = "Server=localhost;Database=blogdb;User Id=sa;Password=YourPass;";
            builder.UseSqlServer(designTimeConnectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
