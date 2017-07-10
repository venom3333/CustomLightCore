using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Specification
    {
        public Specification()
        {
            SpecificationValues = new HashSet<SpecificationValue>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }

        public virtual ICollection<SpecificationValue> SpecificationValues { get; set; }
        public virtual Product Product { get; set; }
    }
}
