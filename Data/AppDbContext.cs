using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product 
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");

                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });

            entity.HasKey(p => p.ProductCode);

            entity.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(p => p.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(p => p.Stock)
                .IsRequired();

            entity.Property(p => p.Category)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired(false);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders", "dbo", table =>
            table.HasCheckConstraint("CK_Orders_Status", "[Status] In ('Pending', 'Confirmed')"));
            entity.HasKey(o => o.OrderId);
            entity.Property(o => o.OrderId).ValueGeneratedOnAdd();
            entity.Property(o => o.OrderNumber).HasMaxLength(35).IsRequired();
            entity.Property(o => o.OrderDate).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(o => o.Status).HasMaxLength(20).IsRequired();
            entity.HasIndex(o => o.OrderDate);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem", "dbo", table =>
            {
                table.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0");
                table.HasCheckConstraint("CK_OrderItem_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.HasKey(i => i.OrderItemId);
            entity.Property(i => i.OrderItemId).ValueGeneratedOnAdd().IsRequired();
            entity.Property(i => i.ProductCode).HasMaxLength(30).IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();
            entity.Property(i => i.Subtotal).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

            entity.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(i => i.ProductCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => new { i.OrderId, i.ProductCode }).IsUnique();
            entity.HasIndex(i => i.ProductCode);

        });
    }
}