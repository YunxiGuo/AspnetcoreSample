using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CookieAuthMvcSample.Data
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseMySQL("Server=132.232.68.150;Port=3306;Database=mvcCookie;User Id=guoyunxi;Password=pwd123");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}