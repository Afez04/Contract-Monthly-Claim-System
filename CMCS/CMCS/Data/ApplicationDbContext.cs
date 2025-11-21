using System;
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Data
{
    class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=AFEZ;Database=CMCSDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Claims).WithOne(c => c.User).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Message>().HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>().HasOne(m => m.Reciever).WithMany().HasForeignKey(m => m.RecieverId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
