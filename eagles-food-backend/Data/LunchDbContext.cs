using eagles_food_backend.Domains.Models;
using Microsoft.EntityFrameworkCore;

namespace eagles_food_backend.Data
{
    public partial class LunchDbContext : DbContext
    {
        public LunchDbContext()
        {
        }

        public LunchDbContext(DbContextOptions<LunchDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lunch> Lunches { get; set; } = null!;
        public virtual DbSet<Organization> Organizations { get; set; } = null!;
        public virtual DbSet<OrganizationInvite> OrganizationInvites { get; set; } = null!;
        public virtual DbSet<OrganizationLunchWallet> OrganizationLunchWallets { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Withdrawal> Withdrawals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Lunch>(entity =>
            {
                entity.ToTable("lunches");

                entity.HasIndex(e => e.OrgId, "org_id");

                entity.HasIndex(e => e.ReceiverId, "receiver_id");

                entity.HasIndex(e => e.SenderId, "sender_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Note)
                    .HasColumnType("text")
                    .HasColumnName("note");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.Redeemed)
                    .HasColumnName("redeemed")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Org)
                    .WithMany(p => p.Lunches)
                    .HasForeignKey(d => d.OrgId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lunches_ibfk_1");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.LunchReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lunches_ibfk_3");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.LunchSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("lunches_ibfk_2");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organizations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(4)
                    .HasColumnName("currency_code");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LunchPrice)
                    .HasPrecision(10, 2)
                    .HasColumnName("lunch_price");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<OrganizationInvite>(entity =>
            {
                entity.ToTable("organization_invites");

                entity.HasIndex(e => e.OrgId, "org_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .HasColumnName("token");

                entity.Property(e => e.Ttl)
                    .HasColumnType("datetime")
                    .HasColumnName("ttl");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Org)
                    .WithMany(p => p.OrganizationInvites)
                    .HasForeignKey(d => d.OrgId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("organization_invites_ibfk_1");
            });

            modelBuilder.Entity<OrganizationLunchWallet>(entity =>
            {
                entity.ToTable("organization_lunch_wallets");

                entity.HasIndex(e => e.OrgId, "org_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balance)
                    .HasPrecision(10, 2)
                    .HasColumnName("balance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Org)
                    .WithMany(p => p.OrganizationLunchWallets)
                    .HasForeignKey(d => d.OrgId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("organization_lunch_wallets_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "email")
                    .IsUnique();

                entity.HasIndex(e => e.OrgId, "org_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(255)
                    .HasColumnName("bank_code");

                entity.Property(e => e.BankName)
                    .HasMaxLength(255)
                    .HasColumnName("bank_name");

                entity.Property(e => e.BankNumber)
                    .HasMaxLength(255)
                    .HasColumnName("bank_number");

                entity.Property(e => e.BankRegion)
                    .HasMaxLength(255)
                    .HasColumnName("bank_region");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Currency)
                    .HasMaxLength(128)
                    .HasColumnName("currency");

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(4)
                    .HasColumnName("currency_code");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .HasColumnName("last_name");

                entity.Property(e => e.LunchCreditBalance).HasColumnName("lunch_credit_balance");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .HasColumnName("password_hash");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.Property(e => e.ProfilePic)
                    .HasMaxLength(255)
                    .HasColumnName("profile_pic");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(255)
                    .HasColumnName("refresh_token");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Org)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OrgId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("users_ibfk_1");
            });

            modelBuilder.Entity<Withdrawal>(entity =>
            {
                entity.ToTable("withdrawals");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Status)
                    .HasColumnType("enum('not_redeemed','redeemed')")
                    .HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Withdrawals)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("withdrawals_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
