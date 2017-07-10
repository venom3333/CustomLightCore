using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class SpecificationValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int SpecificationTitleId { get; set; }
        public int SpecificationId { get; set; }

        public virtual Specification Specification { get; set; }
        public virtual SpecificationTitle SpecificationTitle { get; set; }
    }
}
