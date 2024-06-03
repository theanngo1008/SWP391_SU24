using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BE.Models;

public partial class JewelrySystemDbContext : DbContext
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

    public virtual DbSet<JewelryDetail> JewelryDetails { get; set; }

    public virtual DbSet<JewelryGemstone> JewelryGemstones { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Wage> Wages { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=jewelrysystemserver.database.windows.net;Initial Catalog=JewelrySystemDB;Persist Security Info=True;User ID=JewelrySystemAdmin;Password=FPTUniQ9;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Account__91CBC37842273963");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534BDA9506F").IsUnique();

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
            entity.HasKey(e => e.CateId).HasName("PK__Category__27638D1456B4A7E6");

            entity.ToTable("Category");

            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateName).HasMaxLength(50);
        });

        modelBuilder.Entity<Gemstone>(entity =>
        {
            entity.HasKey(e => e.GemstoneId).HasName("PK__Gemstone__058645531A842F3B");

            entity.ToTable("Gemstone");

            entity.Property(e => e.GemstoneCost).HasColumnType("money");
            entity.Property(e => e.GemstoneName).HasMaxLength(50);
        });

        modelBuilder.Entity<Jewelry>(entity =>
        {
            entity.HasKey(e => e.JewelryId).HasName("PK__Jewelry__807031D55675ED2E");

            entity.ToTable("Jewelry");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.JewelryName).HasMaxLength(100);
            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Quotation).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK__Jewelry__Quotati__76969D2E");

            entity.HasOne(d => d.SubCate).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.SubCateId)
                .HasConstraintName("FK__Jewelry__SubCate__787EE5A0");

            entity.HasOne(d => d.Wages).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.WagesId)
                .HasConstraintName("FK__Jewelry__WagesId__75A278F5");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Jewelry__Warehou__778AC167");
        });

        modelBuilder.Entity<JewelryDetail>(entity =>
        {
            entity.HasKey(e => e.JewelryDetailId).HasName("PK__JewelryD__2B428608483284AD");

            entity.ToTable("JewelryDetail");

            entity.Property(e => e.JewelryDetailName).HasMaxLength(50);
            entity.Property(e => e.SpotPrice).HasColumnType("money");

            entity.HasOne(d => d.Jewelry).WithMany(p => p.JewelryDetails)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryDe__Jewel__7B5B524B");
        });

        modelBuilder.Entity<JewelryGemstone>(entity =>
        {
            entity.HasKey(e => e.JewelryGemstoneId).HasName("PK__JewelryG__A1718D1ADDC28119");

            entity.ToTable("JewelryGemstone");

            entity.HasOne(d => d.Gemstone).WithMany(p => p.JewelryGemstones)
                .HasForeignKey(d => d.GemstoneId)
                .HasConstraintName("FK__JewelryGe__Gemst__01142BA1");

            entity.HasOne(d => d.Jewelry).WithMany(p => p.JewelryGemstones)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryGe__Jewel__00200768");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__C87C037CDEB42F6D");

            entity.ToTable("Message");

            entity.HasIndex(e => e.OrderId, "UQ__Message__C3905BCE9D77BA30").IsUnique();

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Message1).HasColumnName("Message");

            entity.HasOne(d => d.Order).WithOne(p => p.Message)
                .HasForeignKey<Message>(d => d.OrderId)
                .HasConstraintName("FK__Message__OrderId__0E6E26BF");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFF0999371");

            entity.Property(e => e.OrderName).HasMaxLength(255);
            entity.Property(e => e.OrderStatus).HasMaxLength(255);

            entity.HasOne(d => d.Acc).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Orders__AccId__05D8E0BE");

            entity.HasOne(d => d.Shipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingId)
                .HasConstraintName("FK__Orders__Shipping__06CD04F7");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36C9F2A09FB");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.DetailStatus).HasMaxLength(255);

            entity.HasOne(d => d.Jewelry).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__OrderDeta__Jewel__09A971A2");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__0A9D95DB");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.QuotationId).HasName("PK__Quotatio__E1975293675B7E36");

            entity.ToTable("Quotation");

            entity.Property(e => e.QuotationName).HasMaxLength(50);
            entity.Property(e => e.QuotationStatus).HasMaxLength(100);
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Acc).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Quotation__AccId__72C60C4A");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("PK__Shipping__5FACD580C00B0AA2");

            entity.ToTable("Shipping");

            entity.Property(e => e.ShippingName).HasMaxLength(100);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCateId).HasName("PK__SubCateg__2A3F7546A4F569DC");

            entity.ToTable("SubCategory");

            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubCateName).HasMaxLength(50);

            entity.HasOne(d => d.Cate).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__SubCatego__CateI__6E01572D");
        });

        modelBuilder.Entity<Wage>(entity =>
        {
            entity.HasKey(e => e.WagesId).HasName("PK__Wages__016AF0B5831558F5");

            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.WagesName).HasMaxLength(100);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFD9C6FF71D9");

            entity.ToTable("Warehouse");

            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.WarehouseName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
