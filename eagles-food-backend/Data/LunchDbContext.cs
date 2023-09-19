using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Data
{
    public class LunchDbContext : DbContext
    {
        public LunchDbContext(DbContextOptions<LunchDbContext> options) : base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
