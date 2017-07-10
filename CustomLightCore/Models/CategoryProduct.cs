using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class CategoryProduct
    {
        public int CategoriesId { get; set; }
        public int ProductsId { get; set; }

        public virtual Category Categories { get; set; }
        public virtual Product Products { get; set; }
    }
}
