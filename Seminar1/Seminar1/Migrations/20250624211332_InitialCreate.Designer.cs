﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Seminar1.Models;

#nullable disable

namespace Seminar1.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20250624211332_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Seminar1.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ProductName");

                    b.HasKey("Id")
                        .HasName("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ProductCategories", (string)null);
                });

            modelBuilder.Entity("Seminar1.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("Cost")
                        .HasColumnType("integer")
                        .HasColumnName("Cost");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("ProductName");

                    b.HasKey("Id")
                        .HasName("ProductID");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Seminar1.Models.ProductStorage", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("integer");

                    b.Property<int>("StorageID")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int?>("StorageId")
                        .HasColumnType("integer");

                    b.HasKey("ProductID", "StorageID");

                    b.HasIndex("ProductId");

                    b.HasIndex("StorageID");

                    b.HasIndex("StorageId");

                    b.ToTable("StorageProduct", (string)null);
                });

            modelBuilder.Entity("Seminar1.Models.Storage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("ProductCount");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("StorageName");

                    b.HasKey("Id")
                        .HasName("StorageID");

                    b.ToTable("Storage", (string)null);
                });

            modelBuilder.Entity("Seminar1.Models.Product", b =>
                {
                    b.HasOne("Seminar1.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("CategoryToProduct");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Seminar1.Models.ProductStorage", b =>
                {
                    b.HasOne("Seminar1.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Seminar1.Models.Product", null)
                        .WithMany("ProductStorages")
                        .HasForeignKey("ProductId");

                    b.HasOne("Seminar1.Models.Storage", "Storage")
                        .WithMany()
                        .HasForeignKey("StorageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Seminar1.Models.Storage", null)
                        .WithMany("ProductStorages")
                        .HasForeignKey("StorageId");

                    b.Navigation("Product");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("Seminar1.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Seminar1.Models.Product", b =>
                {
                    b.Navigation("ProductStorages");
                });

            modelBuilder.Entity("Seminar1.Models.Storage", b =>
                {
                    b.Navigation("ProductStorages");
                });
#pragma warning restore 612, 618
        }
    }
}
