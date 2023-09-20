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
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Lunch> Lunches { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationWallet> OrganizationWallets { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Organization>().Property(e=>e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasOne(o => o.Organization).WithMany(o => o.users);
            modelBuilder.Entity<Withdrawal>().HasOne(w => w.User).WithMany(u => u.withdrawals);
            modelBuilder.Entity<Invite>().HasOne(i => i.organization).WithMany(o => o.invitations);
            //modelBuilder.Entity<Lunch>().HasOne(l => l.sender).WithMany(s => s.sent_lunches);
            //modelBuilder.Entity<Lunch>().HasOne(l => l.reciever).WithMany(s => s.recieved_lunches);
        }

    }
}
