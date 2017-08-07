
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

    public class ProductViewModel
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

        ///// <summary>
        ///// Gets or sets the specification values.
        ///// </summary>
        //public List<SpecificationValue> SpecificationValues { get; set; }

        ///// <summary>
        ///// Gets or sets the specification value.
        ///// </summary>
        //public SpecificationValue SpecificationValue { get; set; }


        /// <summary>
        /// Gets or sets the product type.
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Gets or sets the product type ID.
        /// </summary>
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
                        .Include(p => p.ProductType)
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


            // Категории проекта
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

                result.Icon = ms.ToArray();
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

                        var image = new ProductImage
                        {
                            ImageData = ms.ToArray(),
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


            return result;
        }

        /// <summary>
        /// Получаем ДатаМодель на основе существующей вью модели
        /// </summary>
        public Product GetModelByViewModel()
        {
            return (Product)this;
        }

        /// <summary>
        /// Приведение экземпляра доменной модели во viewModel.
        /// </summary>
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
                ExistingProductImageIds = item.ProductImages.Select(image => image.Id).ToList(),
                CategoryProductId = item.CategoryProduct.Select(cp => cp.CategoriesId).ToList()
            };
            return result;
        }

        /// <summary>
        /// Получаем ВьюМодель на основе id ДатаМодели
        /// </summary>
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
                              .FirstOrDefaultAsync(p => p.Id == id);
            }
            return (ProductViewModel)product;
        }
    }
}
