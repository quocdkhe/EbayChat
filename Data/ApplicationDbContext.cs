using Microsoft.EntityFrameworkCore;
using EbayChat.Entities;

namespace EbayChat.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ giữa User và Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.sender)
                .WithMany(u => u.Messagesenders)
                .HasForeignKey(m => m.senderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.receiver)
                .WithMany(u => u.Messagereceivers)
                .HasForeignKey(m => m.receiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed dữ liệu mẫu
            modelBuilder.Entity<User>().HasData(
                new User { id = 1, username = "Alice", email = "alice@example.com"},
                new User { id = 2, username = "Bob", email = "bob@example.com" }
            );
        }
    }
}