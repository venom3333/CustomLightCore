using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CustomLightCore.Models
{
    public partial class CustomLightContext : IdentityDbContext<User, Role, string>
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProduct { get; set; }
        public virtual DbSet<CategoryProject> CategoryProject { get; set; }
        public virtual DbSet<Essential> Essentials { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProjectImage> ProjectImages { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<SpecificationTitle> SpecificationTitles { get; set; }
        public virtual DbSet<SpecificationValue> SpecificationValues { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }

        //public virtual DbSet<User> Users { get; set; }

        public CustomLightContext() : base()
        {
            // пока ничего тут
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CustomLight;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=ms-sql-9.in-solve.ru;Database=1gb_x_custobf7;User=1gb_customlight;Password=zaefa67b9a");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId });
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.UserId });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

            entity.Property(e => e.Updated).HasColumnType("datetime");
        });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(e => new { e.CategoriesId, e.ProductsId
    })
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

            modelBuilder.Entity<Essential>(entity =>
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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

entity.Property(e => e.OrderString).IsRequired();
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.Property(e => e.Alias).IsRequired();

entity.Property(e => e.Created).HasColumnType("datetime");

entity.Property(e => e.Name).IsRequired();

entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductImage>(entity =>
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

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
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

            modelBuilder.Entity<ProjectImage>(entity =>
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

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Created).HasColumnType("datetime");

entity.Property(e => e.Name).IsRequired();

entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Slide>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

entity.Property(e => e.ImageData).IsRequired();

entity.Property(e => e.ImageMimeType).IsRequired();

entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SpecificationTitle>(entity =>
            {
                entity.HasIndex(e => e.ProductTypeId)
                    .HasName("IX_FK_ProductTypeSpecificationTitle");

entity.Property(e => e.Name).IsRequired();

entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.SpecificationTitles)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_ProductTypeSpecificationTitle");
            });

            modelBuilder.Entity<SpecificationValue>(entity =>
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

            modelBuilder.Entity<Specification>(entity =>
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