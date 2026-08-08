using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace garagesales.Models;

public partial class Database1Context : DbContext
{
    public Database1Context()
    {
    }

    public Database1Context(DbContextOptions<Database1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<GarageSale> GarageSales { get; set; }

    public virtual DbSet<GarageSaleItem> GarageSaleItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:GarageSaleDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GarageSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GarageSa__3214EC0700C9EDE1");

            entity.ToTable("GarageSale");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GarageSaleItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GarageSa__3214EC0709F4D0A0");

            entity.ToTable("GarageSaleItem");

            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.GarageSale).WithMany(p => p.GarageSaleItems)
                .HasForeignKey(d => d.GarageSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GarageSal__Garag__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
