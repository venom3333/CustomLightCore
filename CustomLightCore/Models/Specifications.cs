using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Specifications
    {
        public Specifications()
        {
            SpecificationValues = new HashSet<SpecificationValues>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }

        public virtual ICollection<SpecificationValues> SpecificationValues { get; set; }
        public virtual Products Product { get; set; }
    }
}
