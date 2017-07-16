using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CustomLightCore.Models;

namespace CustomLightCore.Migrations
{
    [DbContext(typeof(CustomLightContext))]
    [Migration("20170716175009_ChangeUserLoginKey")]
    partial class ChangeUserLoginKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CustomLightCore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Icon");

                    b.Property<string>("IconMimeType");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ShortDescription");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CustomLightCore.Models.CategoryProduct", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnName("Categories_Id");

                    b.Property<int>("ProductsId")
                        .HasColumnName("Products_Id");

                    b.HasKey("CategoriesId", "ProductsId")
                        .HasName("PK_CategoryProduct");

                    b.HasIndex("ProductsId")
                        .HasName("IX_FK_CategoryProduct_Product");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("CustomLightCore.Models.CategoryProject", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnName("Categories_Id");

                    b.Property<int>("ProjectsId")
                        .HasColumnName("Projects_Id");

                    b.HasKey("CategoriesId", "ProjectsId")
                        .HasName("PK_CategoryProject");

                    b.HasIndex("ProjectsId")
                        .HasName("IX_FK_CategoryProject_Project");

                    b.ToTable("CategoryProject");
                });

            modelBuilder.Entity("CustomLightCore.Models.Essentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About")
                        .IsRequired();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Boss")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<byte[]>("LogoImageData")
                        .IsRequired();

                    b.Property<byte[]>("LogoImageInvertedData")
                        .IsRequired();

                    b.Property<string>("LogoImageInvertedMimeType")
                        .IsRequired();

                    b.Property<string>("LogoImageMimeType")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Essentials");
                });

            modelBuilder.Entity("CustomLightCore.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("OrderString")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CustomLightCore.Models.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PageContent");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("CustomLightCore.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Icon");

                    b.Property<string>("IconMimeType");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductTypeId");

                    b.Property<string>("ShortDescription");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("ProductTypeId")
                        .HasName("IX_FK_ProductTypeProduct");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CustomLightCore.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ImageData")
                        .IsRequired();

                    b.Property<string>("ImageMimeType")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .HasName("IX_FK_ProductProductImage");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("CustomLightCore.Models.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("CustomLightCore.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Icon");

                    b.Property<string>("IconMimeType");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ShortDescription");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CustomLightCore.Models.ProjectImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ImageData")
                        .IsRequired();

                    b.Property<string>("ImageMimeType")
                        .IsRequired();

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .HasName("IX_FK_ProjectProjectImage");

                    b.ToTable("ProjectImages");
                });

            modelBuilder.Entity("CustomLightCore.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CustomLightCore.Models.Slide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<byte[]>("ImageData")
                        .IsRequired();

                    b.Property<string>("ImageMimeType")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Slides");
                });

            modelBuilder.Entity("CustomLightCore.Models.Specification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Price");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .HasName("IX_FK_ProductSpecification");

                    b.ToTable("Specifications");
                });

            modelBuilder.Entity("CustomLightCore.Models.SpecificationTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductTypeId");

                    b.HasKey("Id");

                    b.HasIndex("ProductTypeId")
                        .HasName("IX_FK_ProductTypeSpecificationTitle");

                    b.ToTable("SpecificationTitles");
                });

            modelBuilder.Entity("CustomLightCore.Models.SpecificationValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SpecificationId");

                    b.Property<int>("SpecificationTitleId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SpecificationId")
                        .HasName("IX_FK_SpecificationSpecificationValue");

                    b.HasIndex("SpecificationTitleId")
                        .HasName("IX_FK_SpecificationTitleSpecificationValue");

                    b.ToTable("SpecificationValues");
                });

            modelBuilder.Entity("CustomLightCore.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("UserId1");

                    b.HasKey("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("RoleId");

                    b.Property<string>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("CustomLightCore.Models.CategoryProduct", b =>
                {
                    b.HasOne("CustomLightCore.Models.Category", "Categories")
                        .WithMany("CategoryProduct")
                        .HasForeignKey("CategoriesId")
                        .HasConstraintName("FK_CategoryProduct_Category");

                    b.HasOne("CustomLightCore.Models.Product", "Products")
                        .WithMany("CategoryProduct")
                        .HasForeignKey("ProductsId")
                        .HasConstraintName("FK_CategoryProduct_Product");
                });

            modelBuilder.Entity("CustomLightCore.Models.CategoryProject", b =>
                {
                    b.HasOne("CustomLightCore.Models.Category", "Categories")
                        .WithMany("CategoryProject")
                        .HasForeignKey("CategoriesId")
                        .HasConstraintName("FK_CategoryProject_Category");

                    b.HasOne("CustomLightCore.Models.Project", "Projects")
                        .WithMany("CategoryProject")
                        .HasForeignKey("ProjectsId")
                        .HasConstraintName("FK_CategoryProject_Project");
                });

            modelBuilder.Entity("CustomLightCore.Models.Product", b =>
                {
                    b.HasOne("CustomLightCore.Models.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeId")
                        .HasConstraintName("FK_ProductTypeProduct")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CustomLightCore.Models.ProductImage", b =>
                {
                    b.HasOne("CustomLightCore.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ProductProductImage")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CustomLightCore.Models.ProjectImage", b =>
                {
                    b.HasOne("CustomLightCore.Models.Project", "Project")
                        .WithMany("ProjectImages")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_ProjectProjectImage")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CustomLightCore.Models.Specification", b =>
                {
                    b.HasOne("CustomLightCore.Models.Product", "Product")
                        .WithMany("Specifications")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ProductSpecification")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CustomLightCore.Models.SpecificationTitle", b =>
                {
                    b.HasOne("CustomLightCore.Models.ProductType", "ProductType")
                        .WithMany("SpecificationTitles")
                        .HasForeignKey("ProductTypeId")
                        .HasConstraintName("FK_ProductTypeSpecificationTitle")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CustomLightCore.Models.SpecificationValue", b =>
                {
                    b.HasOne("CustomLightCore.Models.Specification", "Specification")
                        .WithMany("SpecificationValues")
                        .HasForeignKey("SpecificationId")
                        .HasConstraintName("FK_SpecificationSpecificationValue")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CustomLightCore.Models.SpecificationTitle", "SpecificationTitle")
                        .WithMany("SpecificationValues")
                        .HasForeignKey("SpecificationTitleId")
                        .HasConstraintName("FK_SpecificationTitleSpecificationValue");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CustomLightCore.Models.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CustomLightCore.Models.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CustomLightCore.Models.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CustomLightCore.Models.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CustomLightCore.Models.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
