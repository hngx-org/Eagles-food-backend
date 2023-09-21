using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Data
{
    public class LunchDbContext : DbContext
    {
        public LunchDbContext(DbContextOptions<LunchDbContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Invite> organization_invites { get; set; }
        public DbSet<Lunch> lunches { get; set; }
        public DbSet<Organization> organizations { get; set; }
        public DbSet<OrganizationWallet> organization_lunch_wallets { get; set; }
        public DbSet<Withdrawal> withdrawals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasOne(o => o.Organization).WithMany(o => o.users);
            modelBuilder.Entity<Withdrawal>().HasOne(w => w.User).WithMany(u => u.withdrawals);
            modelBuilder.Entity<Invite>().HasOne(i => i.organization).WithMany(o => o.invitations);

            modelBuilder.Entity<Lunch>().HasOne(l => l.sender).WithMany(s => s.sent_lunches);
            modelBuilder.Entity<Lunch>().HasOne(l => l.reciever).WithMany(s => s.recieved_lunches);
            modelBuilder.Entity<Lunch>().HasOne(l => l.Organization).WithMany(o => o.lunches);
        }
    }
}
