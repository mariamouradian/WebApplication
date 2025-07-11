﻿using Microsoft.EntityFrameworkCore;
using Seminar1.Models;

public class ProductContext : DbContext
{
    public ProductContext()
    {
    }

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public virtual DbSet<ProductStorage> ProductStorages { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(x => x.Id).HasName("ProductID");
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(e => e.Name)
                  .HasColumnName("ProductName")
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(e => e.Description)
                  .HasColumnName("Description")
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(e => e.Cost)
                  .HasColumnName("Cost")
                  .IsRequired();

            entity.HasOne(x => x.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(x => x.CategoryId)
                  .HasConstraintName("CategoryToProduct");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("ProductCategories");
            entity.HasKey(x => x.Id).HasName("CategoryId");
            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(e => e.Name)
                  .HasColumnName("ProductName")
                  .HasMaxLength(255)
                  .IsRequired();
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.ToTable("Storage");
            entity.HasKey(x => x.Id).HasName("StorageID");

            entity.Property(e => e.Name)
                  .HasColumnName("StorageName")
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(e => e.Count)
                  .HasColumnName("ProductCount");

            entity.HasMany(x => x.Products)
                  .WithMany(p => p.Storages)
                  .UsingEntity<ProductStorage>(
                      j => j.HasOne(ps => ps.Product)
                            .WithMany()
                            .HasForeignKey(ps => ps.ProductID),
                      j => j.HasOne(ps => ps.Storage)
                            .WithMany()
                            .HasForeignKey(ps => ps.StorageID),
                      j => j.ToTable("StorageProduct"));
        });

        modelBuilder.Entity<ProductStorage>(entity =>
        {
            entity.HasKey(ps => new { ps.ProductID, ps.StorageID });
        });
    }
}