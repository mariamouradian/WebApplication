using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Seminar3.Models;

namespace Seminar3
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<StockItem> StockItems { get; set; } = null!;


        private readonly string? _connectionString;

        public AppDbContext()
        {
        }

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<ProductEntity> Products { get; set; } = null!;
        public virtual DbSet<StorageEntity> Storages { get; set; } = null!;
        public virtual DbSet<CategoryEntity> Categories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString ?? "Host=localhost;Port=5433;Database=my_GraphQL_db;Username=postgres;Password=Example")
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();
                entity.Property(e => e.Description)
                      .HasMaxLength(255)
                      .IsRequired();
                entity.Property(e => e.Price)
                      .IsRequired();
                entity.HasOne(x => x.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(x => x.CategoryId);
                entity.HasOne(x => x.Storage)
                      .WithMany(s => s.Products)
                      .HasForeignKey(x => x.StorageId);
            });

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Description).IsUnique();

                entity.Property(e => e.Description)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();
            });

            modelBuilder.Entity<StorageEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Name)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasMaxLength(255)
                      .IsRequired();
            });

            modelBuilder.Entity<StockItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Product)
                      .WithMany()
                      .HasForeignKey(x => x.ProductId);
                entity.HasOne(x => x.Storage)
                      .WithMany()
                      .HasForeignKey(x => x.StorageId);
            });
        }
    }
}