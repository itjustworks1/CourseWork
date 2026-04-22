using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace API.NewDB;

public partial class _113526KrylovKursovaiContext : DbContext
{
    public _113526KrylovKursovaiContext()
    {
    }

    public _113526KrylovKursovaiContext(DbContextOptions<_113526KrylovKursovaiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStructure> OrderStructures { get; set; }

    public virtual DbSet<Parameter> Parameters { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductParameter> ProductParameters { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("userid=student;password=student;server=192.168.200.13;database=1135_26_KrylovKursovai", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.UserId, "FK_Order_UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_UserId");
        });

        modelBuilder.Entity<OrderStructure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("OrderStructure");

            entity.HasIndex(e => e.OrderId, "FK_OrderStructure_OrderId");

            entity.HasIndex(e => e.ProductId, "FK_OrderStructure_ProductId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.OrderId).HasColumnType("int(11)");
            entity.Property(e => e.ProductId).HasColumnType("int(11)");
            entity.Property(e => e.Quantity).HasColumnType("int(16)");
            entity.Property(e => e.Value).HasPrecision(10);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStructures)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStructure_OrderId");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderStructures)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderStructure_ProductId");
        });

        modelBuilder.Entity<Parameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Parameter");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ProductTypeId, "FK_Product_ProductTypeId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ProductTypeId).HasColumnType("int(11)");
            entity.Property(e => e.Quantity).HasColumnType("int(16)");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Value).HasPrecision(10);

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductTypeId");
        });

        modelBuilder.Entity<ProductParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductParameter");

            entity.HasIndex(e => e.ParameterId, "FK_ProductParameter_ParameterId");

            entity.HasIndex(e => e.ProductId, "FK_ProductParameter_ProductId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Meaning).HasMaxLength(255);
            entity.Property(e => e.ParameterId).HasColumnType("int(11)");
            entity.Property(e => e.ProductId).HasColumnType("int(11)");

            entity.HasOne(d => d.Parameter).WithMany(p => p.ProductParameters)
                .HasForeignKey(d => d.ParameterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductParameter_ParameterId");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductParameters)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductParameter_ProductId");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductType");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasMany(d => d.Parameters).WithMany(p => p.ProductTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductTypeParameter",
                    r => r.HasOne<Parameter>().WithMany()
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductTypeParameter_ParameterId"),
                    l => l.HasOne<ProductType>().WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductTypeParameter_ProductTypeId"),
                    j =>
                    {
                        j.HasKey("ProductTypeId", "ParameterId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("ProductTypeParameter");
                        j.HasIndex(new[] { "ParameterId" }, "FK_ProductTypeParameter_ParameterId");
                        j.IndexerProperty<int>("ProductTypeId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("ParameterId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
