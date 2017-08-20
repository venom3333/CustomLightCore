using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    using System.ComponentModel;

    public partial class Product
    {
        public Product()
        {
            CategoryProduct = new HashSet<CategoryProduct>();
            ProductImages = new HashSet<ProductImage>();
            Specifications = new HashSet<Specification>();
        }

        public int Id { get; set; }

        [DisplayName("Наименование")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }

        [DisplayName("Краткое описание")]
        public string ShortDescription { get; set; }

        [DisplayName("Миниатюра")]
        public byte[] Icon { get; set; }
        public string IconMimeType { get; set; }

        [DisplayName("Опубликовано")]
        public bool IsPublished { get; set; }

        [DisplayName("Создано")]
        //public DateTime? Created { get; set; }
        public DateTime Created
        {
            get => _created ?? DateTime.Now;

            set => _created = value;
        }

        private DateTime? _created;

        [DisplayName("Обновлено")]
        public DateTime Updated { get; set; }
        public int ProductTypeId { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProduct { get; set; }

        [DisplayName("Текущие изображения")]
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        [DisplayName("Текущие спецификации")]
        public virtual ICollection<Specification> Specifications { get; set; }

        [DisplayName("Тип продукта")]
        public virtual ProductType ProductType { get; set; }
    }
}
