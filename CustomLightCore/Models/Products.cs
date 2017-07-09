﻿using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Products
    {
        public Products()
        {
            CategoryProduct = new HashSet<CategoryProduct>();
            ProductImages = new HashSet<ProductImages>();
            Specifications = new HashSet<Specifications>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public byte[] Icon { get; set; }
        public string IconMimeType { get; set; }
        public bool IsPublished { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int ProductTypeId { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProduct { get; set; }
        public virtual ICollection<ProductImages> ProductImages { get; set; }
        public virtual ICollection<Specifications> Specifications { get; set; }
        public virtual ProductTypes ProductType { get; set; }
    }
}
