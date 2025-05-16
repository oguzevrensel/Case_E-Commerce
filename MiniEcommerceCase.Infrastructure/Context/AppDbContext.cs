using Microsoft.EntityFrameworkCore;
using MiniEcommerceCase.Domain.Entities;

namespace MiniEcommerceCase.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
