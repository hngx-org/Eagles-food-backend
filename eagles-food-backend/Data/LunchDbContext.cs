    using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Data
{
    public class LunchDbContext : DbContext
    {
        public LunchDbContext(DbContextOptions<LunchDbContext> options) : base(options)
        {
                
        }

<<<<<<< HEAD
        public DbSet<User> users { get; set; }
        public DbSet<Invite> organization_invites { get; set; }
        public DbSet<Lunch> lunches { get; set; }
        public DbSet<Organization> organizations { get; set; }
        public DbSet<OrganizationWallet> organization_lunch_wallets { get; set; }
        public DbSet<Withdawal> withdrawals { get; set; }
=======
        public DbSet<User> Users { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Lunch> Lunches { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationWallet> OrganizationWallets { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Organization>().Property(e=>e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasOne(o => o.Organization).WithMany(o => o.users);
            modelBuilder.Entity<Withdrawal>().HasOne(w => w.User).WithMany(u => u.withdrawals);
            modelBuilder.Entity<Invite>().HasOne(i => i.organization).WithMany(o => o.invitations);
<<<<<<< HEAD

            modelBuilder.Entity<Lunch>().HasOne(l => l.sender).WithMany(s => s.sent_lunches);
            modelBuilder.Entity<Lunch>().HasOne(l => l.reciever).WithMany(s => s.recieved_lunches);
            modelBuilder.Entity<Lunch>().HasOne(l => l.Organization).WithMany(o => o.lunches);

=======
            //modelBuilder.Entity<Lunch>().HasOne(l => l.sender).WithMany(s => s.sent_lunches);
            //modelBuilder.Entity<Lunch>().HasOne(l => l.reciever).WithMany(s => s.recieved_lunches);
>>>>>>> d578b4b9c03e1e86c6cd0805d08b730cfdd32333
        }

    }
}
