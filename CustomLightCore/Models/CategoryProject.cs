using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class CategoryProject
    {
        public int CategoriesId { get; set; }
        public int ProjectsId { get; set; }

        public virtual Category Categories { get; set; }
        public virtual Projects Projects { get; set; }
    }
}
