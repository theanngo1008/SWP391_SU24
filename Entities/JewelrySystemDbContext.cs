using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BE.Entities;

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

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<Gemstone> Gemstones { get; set; }

    public virtual DbSet<Jewelry> Jewelries { get; set; }

    public virtual DbSet<JewelryFee> JewelryFees { get; set; }

    public virtual DbSet<JewelryGemstone> JewelryGemstones { get; set; }

    public virtual DbSet<JewelryGold> JewelryGolds { get; set; }

    public virtual DbSet<MembershipCard> MembershipCards { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<SpotGoldPrice> SpotGoldPrices { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=jewelrysystemserver.database.windows.net;Initial Catalog=JewelrySystemDB;Persist Security Info=True;User ID=jewelrysystemadmin;Password=Kid30032001;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Accounts__91CBC37895018683");

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D1053493CB06F1").IsUnique();

            entity.Property(e => e.AccName).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Deposit).HasColumnType("money");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Categori__27638D147E2FC9F4");

            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateName).HasMaxLength(50);
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.FeeId).HasName("PK__Fees__B387B2298065FC00");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.FeeType).HasMaxLength(100);
        });

        modelBuilder.Entity<Gemstone>(entity =>
        {
            entity.HasKey(e => e.GemstoneId).HasName("PK__Gemstone__05864553776D99BD");

            entity.Property(e => e.GemstoneCost).HasColumnType("money");
            entity.Property(e => e.GemstoneName).HasMaxLength(50);
        });

        modelBuilder.Entity<Jewelry>(entity =>
        {
            entity.HasKey(e => e.JewelryId).HasName("PK__Jewelrie__807031D56B3FB6C3");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.JewelryName).HasMaxLength(100);
            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Quotation).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK__Jewelries__Quota__693CA210");

            entity.HasOne(d => d.SubCate).WithMany(p => p.Jewelries)
                .HasForeignKey(d => d.SubCateId)
                .HasConstraintName("FK__Jewelries__SubCa__6A30C649");
        });

        modelBuilder.Entity<JewelryFee>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Fee).WithMany()
                .HasForeignKey(d => d.FeeId)
                .HasConstraintName("FK__JewelryFe__FeeId__6D0D32F4");

            entity.HasOne(d => d.Jewelry).WithMany()
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryFe__Jewel__6C190EBB");
        });

        modelBuilder.Entity<JewelryGemstone>(entity =>
        {
            entity.HasNoKey();

            entity.HasOne(d => d.Gemstone).WithMany()
                .HasForeignKey(d => d.GemstoneId)
                .HasConstraintName("FK__JewelryGe__Gemst__76969D2E");

            entity.HasOne(d => d.Jewelry).WithMany()
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryGe__Jewel__75A278F5");
        });

        modelBuilder.Entity<JewelryGold>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.GoldPrice).WithMany()
                .HasForeignKey(d => d.GoldPriceId)
                .HasConstraintName("FK__JewelryGo__GoldP__71D1E811");

            entity.HasOne(d => d.Jewelry).WithMany()
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__JewelryGo__Jewel__70DDC3D8");
        });

        modelBuilder.Entity<MembershipCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PK__Membersh__55FECDAE0BCE23BA");

            entity.ToTable("MembershipCard");

            entity.HasIndex(e => e.AccId, "UQ__Membersh__91CBC379F2E8676A").IsUnique();

            entity.HasIndex(e => e.CardNumber, "UQ__Membersh__A4E9FFE923955ADE").IsUnique();

            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MembershipLevel).HasMaxLength(50);
            entity.Property(e => e.Points).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Acc).WithOne(p => p.MembershipCard)
                .HasForeignKey<MembershipCard>(d => d.AccId)
                .HasConstraintName("FK__Membershi__AccId__05D8E0BE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF65E717A2");

            entity.Property(e => e.OrderStatus).HasMaxLength(50);

            entity.HasOne(d => d.Acc).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Orders__AccId__7B5B524B");

            entity.HasOne(d => d.Shipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingId)
                .HasConstraintName("FK__Orders__Shipping__7C4F7684");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36CC5A78C93");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DetailStatus).HasMaxLength(50);

            entity.HasOne(d => d.Jewelry).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.JewelryId)
                .HasConstraintName("FK__OrderDeta__Jewel__7F2BE32F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__00200768");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.QuotationId).HasName("PK__Quotatio__E1975293712C9295");

            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuotationName).HasMaxLength(50);
            entity.Property(e => e.QuotationStatus).HasMaxLength(100);
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Acc).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Quotation__AccId__66603565");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("PK__Shipping__5FACD58063F55D3F");

            entity.Property(e => e.ShippingName).HasMaxLength(100);
        });

        modelBuilder.Entity<SpotGoldPrice>(entity =>
        {
            entity.HasKey(e => e.GoldPriceId).HasName("PK__SpotGold__C2C7860CEA932363");

            entity.Property(e => e.DateRecorded).HasColumnType("datetime");
            entity.Property(e => e.GoldType).HasMaxLength(20);
            entity.Property(e => e.SpotPrice).HasColumnType("money");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCateId).HasName("PK__SubCateg__2A3F75463D208493");

            entity.Property(e => e.SubCateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CateId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubCateName).HasMaxLength(50);

            entity.HasOne(d => d.Cate).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__SubCatego__CateI__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
