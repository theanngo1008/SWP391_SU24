using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BE.Entities;

public partial class JewelrySystemDbContext : IdentityDbContext<ApplicationUser>
{
    public JewelrySystemDbContext()
    {
    }

    public JewelrySystemDbContext(DbContextOptions<JewelrySystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Gemstone> Gemstones { get; set; }

    public virtual DbSet<Jewelry> Jewelries { get; set; }

    public virtual DbSet<JewelryMakingCharge> JewelryMakingCharges { get; set; }

    public virtual DbSet<JewelryMetal> JewelryMetals { get; set; }

    public virtual DbSet<LoyaltyCard> LoyaltyCards { get; set; }

    public virtual DbSet<JewelryGemstone> JewelryGemstones { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<SpotMetalPrice> SpotMetalPrices { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=jewelrysystemserver.database.windows.net;Initial Catalog=JewelrySystemDB;Persist Security Info=True;User ID=JewelrySystemAdmin;Password=FPTUniQ9;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Accounts__91CBC378F7FBD5BB");

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D1053441461045").IsUnique();

            entity.Property(e => e.AccName).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Deposit).HasColumnType("money");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Categori__27638D1456F25D59");

            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateName).HasMaxLength(50);
        });

        modelBuilder.Entity<Gemstone>(entity =>
        {
            entity.HasKey(e => e.GemstoneId).HasName("PK__Gemstone__05864553E88FC8E4");

            entity.Property(e => e.GemstoneCost).HasColumnType("money");
            entity.Property(e => e.GemstoneName).HasMaxLength(50);

        });

        modelBuilder.Entity<JewelryGemstone>(entity =>
        {
            entity.HasKey(e => new { e.JewelryId, e.GemstoneId });

            entity.HasOne(d => d.Gemstone).WithMany(p => p.JewelryGemstones)
                .HasForeignKey(d => d.GemstoneId)
                .HasConstraintName("FK__JewelryGe__Gemst__245D67DE");

            entity.HasOne(d => d.Jewelry).WithMany(p => p.JewelryGemstones)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryGe__Jewel__236943A5");
        });

        modelBuilder.Entity<Jewelry>(entity =>
        {
            entity.HasKey(e => e.JewelryId).HasName("PK__Jewelrie__807031D57F4EC531");

            entity.Property(e => e.JewelryName).HasMaxLength(100);
            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Charge).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.ChargeId)
                .HasConstraintName("FK__Jewelries__Charg__6B24EA82");

            entity.HasOne(d => d.Quotation).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK__Jewelries__Quota__6C190EBB");

            entity.HasOne(d => d.SubCate).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.SubCateId)
                .HasConstraintName("FK__Jewelries__SubCa__6E01572D");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Jewelries__Wareh__6D0D32F4");
        });

        modelBuilder.Entity<JewelryMakingCharge>(entity =>
        {
            entity.HasKey(e => e.ChargeId).HasName("PK__JewelryM__17FC361BB670583F");

            entity.Property(e => e.ChargeName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<JewelryMetal>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Jewelry).WithMany()
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryMe__Jewel__71D1E811");

            entity.HasOne(d => d.MetalPrice).WithMany()
                .HasForeignKey(d => d.MetalPriceId)
                .HasConstraintName("FK__JewelryMe__Metal__72C60C4A");
        });

        modelBuilder.Entity<LoyaltyCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__LoyaltyC__55FECDAE0F841BBC");

            entity.HasIndex(e => e.AccId, "UQ__LoyaltyC__91CBC3793BD880B2").IsUnique();

            entity.Property(e => e.CardName).HasMaxLength(50);

            entity.HasOne(d => d.Acc).WithOne(p => p.LoyaltyCard)
                .HasForeignKey<LoyaltyCard>(d => d.AccId)
                .HasConstraintName("FK__LoyaltyCa__AccId__07C12930");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFE459E18C");

            entity.Property(e => e.OrderStatus).HasMaxLength(50);

            entity.HasOne(d => d.Acc).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Orders__AccId__7A672E12");

            entity.HasOne(d => d.Shipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingId)
                .HasConstraintName("FK__Orders__Shipping__7B5B524B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C210EE62C");

            entity.Property(e => e.DetailStatus).HasMaxLength(50);

            entity.HasOne(d => d.Jewelry).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__OrderDeta__Jewel__7E37BEF6");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__7F2BE32F");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.QuotationId).HasName("PK__Quotatio__E197529315E13AA5");

            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuotationName).HasMaxLength(50);
            entity.Property(e => e.QuotationStatus).HasMaxLength(100);
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Acc).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Quotation__AccId__68487DD7");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("PK__Shipping__5FACD5808D86ADCB");

            entity.Property(e => e.ShippingName).HasMaxLength(100);
        });

        modelBuilder.Entity<SpotMetalPrice>(entity =>
        {
            entity.HasKey(e => e.MetalPriceId).HasName("PK__SpotMeta__9601D581348D6B5D");

            entity.Property(e => e.DateRecorded).HasColumnType("datetime");
            entity.Property(e => e.MetalType).HasMaxLength(20);
            entity.Property(e => e.SpotPrice).HasColumnType("money");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCateId).HasName("PK__SubCateg__2A3F754643723416");

            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubCateName).HasMaxLength(50);

            entity.HasOne(d => d.Cate).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__SubCatego__CateI__6383C8BA");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFF9C1983852");

            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.WarehouseName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
