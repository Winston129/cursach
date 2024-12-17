using System;
using System.Collections.Generic;
using cursach.Models;
using Microsoft.EntityFrameworkCore;

namespace cursach.Data;

public partial class CursachItemsContext : DbContext
{
    public CursachItemsContext()
    {
    }

    public CursachItemsContext(DbContextOptions<CursachItemsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Available> Availables { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<Reserved> Reserveds { get; set; }

    public virtual DbSet<Sold> Solds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=cursach_items;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Available>(entity =>
        {
            entity.HasKey(e => e.AvailableId).HasName("PK__Availabl__BCFE5BA0F0440B3A");

            entity.ToTable("Available");

            entity.Property(e => e.AvailableId).HasColumnName("AvailableID");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Client__E67E1A045D5E8B67");

            entity.ToTable("Client");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PassportData).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E83EB16292A56");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.AvailableId).HasColumnName("AvailableID");
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReservedId).HasColumnName("ReservedID");
            entity.Property(e => e.SoldId).HasColumnName("SoldID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Available).WithMany(p => p.Items)
                .HasForeignKey(d => d.AvailableId)
                .HasConstraintName("FK_Item_Available");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_Item_ItemType");

            entity.HasOne(d => d.Reserved).WithMany(p => p.Items)
                .HasForeignKey(d => d.ReservedId)
                .HasConstraintName("FK_Item_Reserved");

            entity.HasOne(d => d.Sold).WithMany(p => p.Items)
                .HasForeignKey(d => d.SoldId)
                .HasConstraintName("FK_Item_Sold");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId).HasName("PK__ItemType__F51540DB58A0EC85");

            entity.ToTable("ItemType");

            entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
            entity.Property(e => e.NameType).HasMaxLength(50);
        });

        modelBuilder.Entity<Reserved>(entity =>
        {
            entity.HasKey(e => e.ReservedId).HasName("PK__Reserved__4E080CE62D5EE024");

            entity.ToTable("Reserved");

            entity.Property(e => e.ReservedId).HasColumnName("ReservedID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Client).WithMany(p => p.Reserveds)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Reserved_Client");
        });

        modelBuilder.Entity<Sold>(entity =>
        {
            entity.HasKey(e => e.SoldId).HasName("PK__Sold__3F0E6689842EFECE");

            entity.ToTable("Sold");

            entity.Property(e => e.SoldId).HasColumnName("SoldID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");

            entity.HasOne(d => d.Client).WithMany(p => p.Solds)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Sold_Client");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
