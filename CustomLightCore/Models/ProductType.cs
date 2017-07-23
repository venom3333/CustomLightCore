using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
            SpecificationTitles = new HashSet<SpecificationTitle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual HashSet<SpecificationTitle> SpecificationTitles { get; set; }
    }
}
