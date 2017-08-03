namespace CustomLightCore.ViewModels.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    public class SpecificationViewModel
    {
        public int Price { get; set; }
        public List<SpecificationValue> SpecificationValues { get; set; }
    }
}
