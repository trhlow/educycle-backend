using EduCycle.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Api.Infrastructure.Data;

public class EduCycleDbContext : DbContext
{
    public EduCycleDbContext(DbContextOptions<EduCycleDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Buyer)
            .WithMany()
            .HasForeignKey(t => t.BuyerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Seller)
            .WithMany()
            .HasForeignKey(t => t.SellerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Giáo trình đại cương" },
            new Category { Id = 2, Name = "Chuyên ngành CNTT" },
            new Category { Id = 3, Name = "Kinh tế - Quản trị" },
            new Category { Id = 4, Name = "Ngoại ngữ" },
            new Category { Id = 5, Name = "Khác" }
        );

    }
}
