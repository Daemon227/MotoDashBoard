using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DashBoard_MotoManager.Datas;

public partial class MotoWebsiteContext : DbContext
{
    public MotoWebsiteContext()
    {
    }

    public MotoWebsiteContext(DbContextOptions<MotoWebsiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<LibraryImage> LibraryImages { get; set; }

    public virtual DbSet<MotoBike> MotoBikes { get; set; }

    public virtual DbSet<MotoLibrary> MotoLibraries { get; set; }

    public virtual DbSet<MotoType> MotoTypes { get; set; }

    public virtual DbSet<MotoVersion> MotoVersions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VersionColor> VersionColors { get; set; }

    public virtual DbSet<VersionImage> VersionImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.MaHangSanXuat).HasName("PK__Brand__977119FC66E9C868");

            entity.ToTable("Brand");

            entity.Property(e => e.MaHangSanXuat).HasMaxLength(50);
            entity.Property(e => e.TenHangSanXuat).HasMaxLength(100);
            entity.Property(e =>e.QuocGia).HasMaxLength(100);
            entity.Property(e => e.MoTaNgan).HasColumnType("NVARCHAR(MAX)");
        });

        modelBuilder.Entity<LibraryImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__LibraryI__7516F70C79ECC013");

            entity.ToTable("LibraryImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MaLibrary).HasMaxLength(50);

            entity.HasOne(d => d.MaLibraryNavigation).WithMany(p => p.LibraryImages)
                .HasForeignKey(d => d.MaLibrary)
                .HasConstraintName("FK__LibraryIm__MaLib__08B54D69");
        });

        modelBuilder.Entity<MotoBike>(entity =>
        {
            entity.HasKey(e => e.MaXe).HasName("PK__MotoBike__272520CDD3089534");

            entity.ToTable("MotoBike");

            entity.Property(e => e.MaXe).HasMaxLength(50);
            entity.Property(e => e.AnhMoTaUrl).HasMaxLength(255);
            entity.Property(e => e.CongSuatToiDa).HasMaxLength(255);
            entity.Property(e => e.DoCaoGamXe).HasMaxLength(255);
            entity.Property(e => e.DoCaoYen).HasMaxLength(255);
            entity.Property(e => e.DungTichBinhXang).HasMaxLength(255);
            entity.Property(e => e.DungTichXyLanh).HasMaxLength(255);
            entity.Property(e => e.DuongKinhHanhTrinhPittong).HasMaxLength(255);
            entity.Property(e => e.GiaBanMoTa).HasMaxLength(50);
            entity.Property(e => e.HeThongKhoiDong).HasMaxLength(255);
            entity.Property(e => e.KhoangCachTrucBanhXe).HasMaxLength(255);
            entity.Property(e => e.KichCoLop).HasMaxLength(255);
            entity.Property(e => e.KichThuoc).HasMaxLength(255);
            entity.Property(e => e.LoaiDongCo).HasMaxLength(255);
            entity.Property(e => e.MaHangSanXuat).HasMaxLength(50);
            entity.Property(e => e.MaLibrary).HasMaxLength(50);
            entity.Property(e => e.MaLoai).HasMaxLength(50);
            entity.Property(e => e.MomentCucDai).HasMaxLength(255);
            entity.Property(e => e.MucTieuThuNhienLieu).HasMaxLength(255);
            entity.Property(e => e.PhuocSau).HasMaxLength(255);
            entity.Property(e => e.PhuocTruoc).HasMaxLength(255);
            entity.Property(e => e.TenXe).HasMaxLength(100);
            entity.Property(e => e.TrongLuong).HasMaxLength(255);
            entity.Property(e => e.TySoNen).HasMaxLength(255);

            entity.HasOne(d => d.MaHangSanXuatNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaHangSanXuat)
                .HasConstraintName("FK__MotoBike__MaHang__0D7A0286");

            entity.HasOne(d => d.MaLibraryNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaLibrary)
                .HasConstraintName("FK__MotoBike__MaLibr__0B91BA14");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.MotoBikes)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__MotoBike__MaLoai__0C85DE4D");
        });

        modelBuilder.Entity<MotoLibrary>(entity =>
        {
            entity.HasKey(e => e.MaLibrary).HasName("PK__MotoLibr__FEC93B4946783D94");

            entity.ToTable("MotoLibrary");

            entity.Property(e => e.MaLibrary).HasMaxLength(50);
        });

        modelBuilder.Entity<MotoType>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__MotoType__730A5759F23C7A47");

            entity.ToTable("MotoType");

            entity.Property(e => e.MaLoai).HasMaxLength(50);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
            entity.Property(e => e.DoiTuongSuDung).HasMaxLength(100);
            entity.Property(e => e.MoTaNgan).HasColumnType("NVARCHAR(MAX)");
        });

        modelBuilder.Entity<MotoVersion>(entity =>
        {
            entity.HasKey(e => e.MaVersion).HasName("PK__MotoVers__9F72C1E1706C9108");

            entity.ToTable("MotoVersion");

            entity.Property(e => e.MaVersion).HasMaxLength(50);
            entity.Property(e => e.GiaBanVersion).HasMaxLength(50);
            entity.Property(e => e.MaXe).HasMaxLength(50);
            entity.Property(e => e.TenVersion).HasMaxLength(100);

            entity.HasOne(d => d.MaXeNavigation).WithMany(p => p.MotoVersions)
                .HasForeignKey(d => d.MaXe)
                .HasConstraintName("FK__MotoVersio__MaXe__2180FB33");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C58834957");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4AD0DFC75").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(25);
        });

        modelBuilder.Entity<VersionColor>(entity =>
        {
            entity.HasKey(e => e.MaVersionColor).HasName("PK__VersionC__2473925C38977191");

            entity.ToTable("VersionColor");

            entity.Property(e => e.MaVersionColor).HasMaxLength(50);
            entity.Property(e => e.MaVersion).HasMaxLength(50);
            entity.Property(e => e.TenMau).HasMaxLength(50);

            entity.HasOne(d => d.MaVersionNavigation).WithMany(p => p.VersionColors)
                .HasForeignKey(d => d.MaVersion)
                .HasConstraintName("FK__VersionCo__MaVer__29221CFB");
        });

        modelBuilder.Entity<VersionImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__VersionI__7516F70C6F950A74");

            entity.ToTable("VersionImage");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MaVersionColor).HasMaxLength(50);

            entity.HasOne(d => d.MaVersionColorNavigation).WithMany(p => p.VersionImages)
                .HasForeignKey(d => d.MaVersionColor)
                .HasConstraintName("FK__VersionIm__MaVer__2BFE89A6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
