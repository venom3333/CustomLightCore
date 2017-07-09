using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class ProductTypes
    {
        public ProductTypes()
        {
            Products = new HashSet<Products>();
            SpecificationTitles = new HashSet<SpecificationTitles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; }
        public virtual ICollection<SpecificationTitles> SpecificationTitles { get; set; }
    }
}
