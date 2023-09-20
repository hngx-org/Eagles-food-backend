﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eagles_food_backend.Data;

#nullable disable

namespace eagles_food_backend.Migrations
{
    [DbContext(typeof(LunchDbContext))]
    [Migration("20230920035502_last_migration")]
    partial class last_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Invite", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("OrganizationId")
                        .HasColumnType("bigint");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Lunch", b =>
                {
                    b.Property<long>("LunchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<long>("recieverId")
                        .HasColumnType("bigint");

                    b.Property<long>("senderId")
                        .HasColumnType("bigint");

                    b.HasKey("LunchId");

                    b.HasIndex("recieverId");

                    b.HasIndex("senderId");

                    b.ToTable("Lunches");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Organization", b =>
                {
                    b.Property<long>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("lunch_price")
                        .HasColumnType("double");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("OrganizationId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.OrganizationWallet", b =>
                {
                    b.Property<long>("WalletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("OrganizationId")
                        .HasColumnType("bigint");

                    b.Property<double>("balance")
                        .HasColumnType("double");

                    b.HasKey("WalletId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationWallets");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("OrganizationId")
                        .HasColumnType("bigint");

                    b.Property<string>("bank_code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("bank_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("bank_number")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("is_admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("last_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password_hash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password_salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("profile_picture")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Withdawal", b =>
                {
                    b.Property<long>("WithdrawalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<double>("ammount")
                        .HasColumnType("double");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("WithdrawalId");

                    b.HasIndex("UserId");

                    b.ToTable("Withdrawals");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Invite", b =>
                {
                    b.HasOne("eagles_food_backend.Domains.Models.Organization", "organization")
                        .WithMany("invitations")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("organization");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Lunch", b =>
                {
                    b.HasOne("eagles_food_backend.Domains.Models.Organization", "reciever")
                        .WithMany("recieved_lunches")
                        .HasForeignKey("recieverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eagles_food_backend.Domains.Models.Organization", "sender")
                        .WithMany("sent_lunches")
                        .HasForeignKey("senderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("reciever");

                    b.Navigation("sender");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.OrganizationWallet", b =>
                {
                    b.HasOne("eagles_food_backend.Domains.Models.Organization", "organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("organization");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.User", b =>
                {
                    b.HasOne("eagles_food_backend.Domains.Models.Organization", "Organization")
                        .WithMany("users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Withdawal", b =>
                {
                    b.HasOne("eagles_food_backend.Domains.Models.User", "User")
                        .WithMany("withdrawals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.Organization", b =>
                {
                    b.Navigation("invitations");

                    b.Navigation("recieved_lunches");

                    b.Navigation("sent_lunches");

                    b.Navigation("users");
                });

            modelBuilder.Entity("eagles_food_backend.Domains.Models.User", b =>
                {
                    b.Navigation("withdrawals");
                });
#pragma warning restore 612, 618
        }
    }
}