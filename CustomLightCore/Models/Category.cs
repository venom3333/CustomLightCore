using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryProduct = new HashSet<CategoryProduct>();
            CategoryProject = new HashSet<CategoryProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public byte[] Icon { get; set; }
        public string IconMimeType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProduct { get; set; }
        public virtual ICollection<CategoryProject> CategoryProject { get; set; }
    }
}
