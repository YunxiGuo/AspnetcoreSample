using CookieAuthMvcSample.Enities;
using Microsoft.EntityFrameworkCore;

namespace CookieAuthMvcSample.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}