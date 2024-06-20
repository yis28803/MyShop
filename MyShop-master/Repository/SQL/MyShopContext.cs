using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Repository;
using Models;

namespace Repository.SQL;

public partial class MyShopContext : DbContext
{
    public MyShopContext()
    {
    }

    public MyShopContext(DbContextOptions<MyShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<ProductSoldCountDay> ProductSoldCountDays { get; set; } = null!;
    public virtual DbSet<ProductSoldCountWeek> ProductSoldCountWeeks { get; set; } = null!;
    public virtual DbSet<ProductSoldCountMonth> ProductSoldCountMonths { get; set; } = null!;
    public virtual DbSet<ProductSoldCountYear> ProductSoldCountYears { get; set; } = null!;

    public virtual DbSet<DayIncome> DayIncomes { get; set; } = null!;
    public virtual DbSet<WeekIncome> WeekIncomes { get; set; } = null!;
    public virtual DbSet<MonthIncome> MonthIncomes { get; set; } = null!;
    public virtual DbSet<YearIncome> YearIncomes { get; set; } = null!;

    public virtual DbSet<ProductSoldCount> ProductSoldCounts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("connectionstring here");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DayIncome>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<WeekIncome>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<MonthIncome>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<YearIncome>(entity => { entity.HasNoKey(); });

        modelBuilder.Entity<ProductSoldCountDay>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<ProductSoldCountWeek>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<ProductSoldCountMonth>(entity => { entity.HasNoKey(); });
        modelBuilder.Entity<ProductSoldCountYear>(entity => { entity.HasNoKey(); });

        modelBuilder.Entity<ProductSoldCount>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_OrderDetails_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetails_ProductId");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.Property(e => e.ImportPrice).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.SalePrice).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
