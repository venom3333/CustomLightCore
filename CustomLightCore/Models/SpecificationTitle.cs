using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class SpecificationTitle
    {
        public SpecificationTitle()
        {
            SpecificationValues = new HashSet<SpecificationValue>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }

        public virtual ICollection<SpecificationValue> SpecificationValues { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
