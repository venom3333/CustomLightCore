using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CustomLightCore.Models
{
    public partial class CustomLightContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProduct { get; set; }
        public virtual DbSet<CategoryProject> CategoryProject { get; set; }
        public virtual DbSet<Essentials> Essentials { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<ProductTypes> ProductTypes { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProjectImages> ProjectImages { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Slides> Slides { get; set; }
        public virtual DbSet<SpecificationTitles> SpecificationTitles { get; set; }
        public virtual DbSet<SpecificationValues> SpecificationValues { get; set; }
        public virtual DbSet<Specifications> Specifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CustomLight;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(e => new { e.CategoriesId, e.ProductsId })
                    .HasName("PK_CategoryProduct");

                entity.HasIndex(e => e.ProductsId)
                    .HasName("IX_FK_CategoryProduct_Product");

                entity.Property(e => e.CategoriesId).HasColumnName("Categories_Id");

                entity.Property(e => e.ProductsId).HasColumnName("Products_Id");

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.CategoryProduct)
                    .HasForeignKey(d => d.CategoriesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryProduct_Category");

                entity.HasOne(d => d.Products)
                    .WithMany(p => p.CategoryProduct)
                    .HasForeignKey(d => d.ProductsId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryProduct_Product");
            });

            modelBuilder.Entity<CategoryProject>(entity =>
            {
                entity.HasKey(e => new { e.CategoriesId, e.ProjectsId })
                    .HasName("PK_CategoryProject");

                entity.HasIndex(e => e.ProjectsId)
                    .HasName("IX_FK_CategoryProject_Project");

                entity.Property(e => e.CategoriesId).HasColumnName("Categories_Id");

                entity.Property(e => e.ProjectsId).HasColumnName("Projects_Id");

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.CategoryProject)
                    .HasForeignKey(d => d.CategoriesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryProject_Category");

                entity.HasOne(d => d.Projects)
                    .WithMany(p => p.CategoryProject)
                    .HasForeignKey(d => d.ProjectsId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryProject_Project");
            });

            modelBuilder.Entity<Essentials>(entity =>
            {
                entity.Property(e => e.About).IsRequired();

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Boss).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.LogoImageData).IsRequired();

                entity.Property(e => e.LogoImageInvertedData).IsRequired();

                entity.Property(e => e.LogoImageInvertedMimeType).IsRequired();

                entity.Property(e => e.LogoImageMimeType).IsRequired();

                entity.Property(e => e.Phone).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.OrderString).IsRequired();
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.Property(e => e.Alias).IsRequired();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductImages>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_ProductProductImage");

                entity.Property(e => e.ImageData).IsRequired();

                entity.Property(e => e.ImageMimeType).IsRequired();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductProductImage");
            });

            modelBuilder.Entity<ProductTypes>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasIndex(e => e.ProductTypeId)
                    .HasName("IX_FK_ProductTypeProduct");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Updated).HasColumnType("datetime");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_ProductTypeProduct");
            });

            modelBuilder.Entity<ProjectImages>(entity =>
            {
                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_FK_ProjectProjectImage");

                entity.Property(e => e.ImageData).IsRequired();

                entity.Property(e => e.ImageMimeType).IsRequired();

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectImages)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectProjectImage");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Slides>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ImageData).IsRequired();

                entity.Property(e => e.ImageMimeType).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SpecificationTitles>(entity =>
            {
                entity.HasIndex(e => e.ProductTypeId)
                    .HasName("IX_FK_ProductTypeSpecificationTitle");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.SpecificationTitles)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_ProductTypeSpecificationTitle");
            });

            modelBuilder.Entity<SpecificationValues>(entity =>
            {
                entity.HasIndex(e => e.SpecificationId)
                    .HasName("IX_FK_SpecificationSpecificationValue");

                entity.HasIndex(e => e.SpecificationTitleId)
                    .HasName("IX_FK_SpecificationTitleSpecificationValue");

                entity.Property(e => e.Value).IsRequired();

                entity.HasOne(d => d.Specification)
                    .WithMany(p => p.SpecificationValues)
                    .HasForeignKey(d => d.SpecificationId)
                    .HasConstraintName("FK_SpecificationSpecificationValue");

                entity.HasOne(d => d.SpecificationTitle)
                    .WithMany(p => p.SpecificationValues)
                    .HasForeignKey(d => d.SpecificationTitleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SpecificationTitleSpecificationValue");
            });

            modelBuilder.Entity<Specifications>(entity =>
            {
                entity.HasIndex(e => e.ProductId)
                    .HasName("IX_FK_ProductSpecification");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Specifications)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductSpecification");
            });
        }
    }
}