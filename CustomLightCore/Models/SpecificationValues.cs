using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class SpecificationValues
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int SpecificationTitleId { get; set; }
        public int SpecificationId { get; set; }

        public virtual Specifications Specification { get; set; }
        public virtual SpecificationTitles SpecificationTitle { get; set; }
    }
}
