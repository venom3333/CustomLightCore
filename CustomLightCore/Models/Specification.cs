using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Specification
    {
        public Specification()
        {
            SpecificationValues = new List<SpecificationValue>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }

        public virtual List<SpecificationValue> SpecificationValues { get; set; }
        public virtual Product Product { get; set; }
    }
}
