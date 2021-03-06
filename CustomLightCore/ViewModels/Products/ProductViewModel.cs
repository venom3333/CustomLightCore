﻿
namespace CustomLightCore.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.Models;
    using CustomLightCore.ViewModels.Specifications;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ProductViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Введите наименование!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Краткое описание")]
        [DataType(DataType.Text)]
        public string ShortDescription { get; set; }

        [DisplayName("Загрузить Иконку")]
        [DataType(DataType.Upload)]
        public IFormFile Icon { get; set; }

        [DisplayName("Опубликовано")]
        public bool IsPublished { get; set; }

        // Категории
        [DisplayName("Категории")]
        public List<int> CategoryProductId { get; set; }

        // Изображения
        [DisplayName("Добавить Изображения")]
        public virtual List<IFormFile> ProductImages { get; set; }

        /// <summary>
        /// Gets or sets the existing product image ids.
        /// </summary>
        [DisplayName("Текущие изображения")]
        public List<int> ExistingProductImageIds { get; set; }


        /// <summary>
        /// Gets or sets Спецификации
        /// </summary>
        [DisplayName("Спецификации")]
        public List<SpecificationViewModel> Specifications { get; set; }

        /// <summary>
        /// Gets or sets Уже существующие Спецификации.
        /// </summary>
        [DisplayName("Текущие спецификации")]
        public List<Specification> ExistingSpecifications { get; set; }

        /// <summary>
        /// Gets or sets the product type.
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product type ID.
        /// </summary>
        [DisplayName("Тип продукта")]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Явное преобразование из вью-модели в доменную.
        /// </summary>
        public static explicit operator Product(ProductViewModel item)
        {
            var now = DateTime.Now;

            var result = new Product();

            if (item.Id != 0)
            {
                using (var db = new CustomLightContext())
                {
                    result = db.Products
                        .Include(p => p.CategoryProduct)
                        .Include(p => p.ProductImages)
                        .Include(p => p.Specifications)
                        .FirstOrDefault(p => p.Id == item.Id);

                    // Удаленные категории удаляем из контекста
                    if (result.CategoryProduct != null)
                    {
                        var categoriesToRemove = new List<CategoryProduct>();
                        foreach (var categoryProduct in result.CategoryProduct)
                        {
                            if (!item.CategoryProductId.Contains(categoryProduct.CategoriesId))
                            {
                                categoriesToRemove.Add(categoryProduct);
                            }
                        }
                        db.CategoryProduct.RemoveRange(categoriesToRemove);
                        db.SaveChanges();
                    }

                    // Удаленные изображения удаляем из контекста
                    if (result.ProductImages != null)
                    {
                        if (item.ExistingProductImageIds != null)
                        {
                            var imagesToRemove = new List<ProductImage>();
                            foreach (var productImage in result.ProductImages)
                            {
                                if (!item.ExistingProductImageIds.Contains(productImage.Id))
                                {
                                    imagesToRemove.Add(productImage);
                                }
                            }
                            db.ProductImages.RemoveRange(imagesToRemove);
                            db.SaveChanges();
                        }
                    }
                }
            }

            result.Name = item.Name;
            result.Description = item.Description;
            result.ShortDescription = item.ShortDescription;
            result.Updated = now;
            result.ProductTypeId = item.ProductTypeId;
            result.IsPublished = item.IsPublished;


            // Категории продукта
            if (item.CategoryProductId != null)
            {
                var categoryProducts = new HashSet<CategoryProduct>();
                foreach (var categoryId in item.CategoryProductId)
                {
                    if (result.CategoryProduct.Select(cp => cp.CategoriesId).ToList().Contains(categoryId))
                    {
                        continue;
                    }
                    var categoryProduct = new CategoryProduct
                    {
                        CategoriesId = categoryId
                    };
                    categoryProducts.Add(categoryProduct);
                }
                result.CategoryProduct = categoryProducts;
            }

            // иконка
            if (item.Icon != null && item.Icon.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream ms = new MemoryStream();
                item.Icon.OpenReadStream().CopyTo(ms);

                // обработка изображения
                var processedImage = ImageProcess(ms.ToArray(), ImageType.Icon);

                result.Icon = processedImage;
                result.IconMimeType = item.Icon.ContentType;
            }

            // изображения
            if (item.ProductImages != null)
            {
                var productImages = new HashSet<ProductImage>();
                foreach (var productImage in item.ProductImages)
                {
                    if (productImage != null && productImage.ContentType.ToLower().StartsWith("image/"))
                    {
                        var ms = new MemoryStream();
                        productImage.OpenReadStream().CopyTo(ms);

                        // обработка изображения
                        var processedImage = ImageProcess(ms.ToArray(), ImageType.Full);

                        var image = new ProductImage
                        {
                            ImageData = processedImage,
                            ImageMimeType = productImage.ContentType
                        };
                        productImages.Add(image);
                    }
                }

                // Теперь добавим то, что осталось в ExistingProductImagesIds
                if (item.ExistingProductImageIds != null)
                {
                    foreach (var imageId in item.ExistingProductImageIds)
                    {
                        using (var db = new CustomLightContext())
                        {
                            var image = db.ProductImages.Find(imageId);
                            productImages.Add(image);
                        }
                    }
                }
                result.ProductImages = productImages;
            }

            // спецификаци
            if (item.Specifications != null)
            {
                var specifications = new HashSet<Specification>();
                foreach (var specification in item.Specifications)
                {
                    var spec = new Specification
                    {
                        Price = specification.Price,
                        SpecificationValues = specification.SpecificationValues
                    };
                    specifications.Add(spec);
                }

                result.Specifications = specifications;
            }

            // Удалим из контекста предыдущие спецификации
            using (var db = new CustomLightContext())
            {
                var existingProductSpecs = db.Specifications.AsNoTracking().Where(s => s.ProductId == item.Id);

                db.Specifications.RemoveRange(existingProductSpecs);
                db.SaveChanges();
            }

            // Для правильной записи SpecificationTitles
            if (result.Specifications != null)
            {
                using (var db = new CustomLightContext())
                {
                    var productType = db.ProductTypes.AsNoTracking()
                        .Include(pt => pt.SpecificationTitles)
                        .FirstOrDefault(pt => pt.Id == result.ProductTypeId);
                    foreach (var spec in result.Specifications)
                    {
                        for (int i = 0; i < productType.SpecificationTitles.Count; i++)
                        {
                            spec.SpecificationValues[i].SpecificationTitleId = db.SpecificationTitles.FirstOrDefault(title =>
                                title.Id == db.ProductTypes.FirstOrDefault(pt => pt.Id == item.ProductTypeId)
                                    .SpecificationTitles.ToList()[i].Id).Id;
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Приведение экземпляра доменной модели во viewModel.
        /// </summary>
        /// /// <param name="item">
        /// The item
        /// </param>
        public static explicit operator ProductViewModel(Product item)
        {
            if (item == null)
            {
                return null;
            }

            ProductViewModel result = new ProductViewModel
            {
                Id = item.Id,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Name = item.Name,
                IsPublished = item.IsPublished,
                ProductTypeId = item.ProductTypeId,
                ProductType = item.ProductType,
                ExistingProductImageIds = item.ProductImages.Select(image => image.Id).ToList(),
                CategoryProductId = item.CategoryProduct.Select(cp => cp.CategoriesId).ToList()
            };

            result.Specifications = new List<SpecificationViewModel>();
            foreach (var itemSpecification in item.Specifications)
            {
                var resultSpecification = new SpecificationViewModel
                {
                    Id = itemSpecification.Id,
                    Price = itemSpecification.Price,
                    SpecificationValues = itemSpecification.SpecificationValues
                };
                result.Specifications.Add(resultSpecification);
            }

            return result;
        }

        /// <summary>
        /// Получаем ВьюМодель на основе id ДатаМодели
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<ProductViewModel> GetViewModelByModelId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var product = new Product();
            using (var db = new CustomLightContext())
            {
                product = await db.Products
                              .Include(p => p.CategoryProduct)
                              .Include(p => p.ProductImages)
                              .Include(p => p.ProductType)
                                .ThenInclude(pt => pt.SpecificationTitles)
                              .Include(p => p.Specifications)
                                .ThenInclude(s => s.SpecificationValues)
                              .FirstOrDefaultAsync(p => p.Id == id);
            }
            return (ProductViewModel)product;
        }

        /// <summary>
        /// Получаем ДатаМодель на основе существующей вью модели
        /// </summary>
        /// <returns>
        /// The <see cref="Product"/>.
        /// </returns>
        public Product GetModelByViewModel()
        {
            return (Product)this;
        }
    }
}
