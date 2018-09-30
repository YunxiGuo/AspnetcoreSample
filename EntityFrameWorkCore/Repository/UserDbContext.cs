using EntityFrameWorkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameWorkCore.Repository
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}