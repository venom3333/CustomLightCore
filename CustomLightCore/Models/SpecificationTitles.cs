using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class SpecificationTitles
    {
        public SpecificationTitles()
        {
            SpecificationValues = new HashSet<SpecificationValues>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }

        public virtual ICollection<SpecificationValues> SpecificationValues { get; set; }
        public virtual ProductTypes ProductType { get; set; }
    }
}
