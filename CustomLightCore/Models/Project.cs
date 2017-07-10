using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Project
    {
        public Project()
        {
            CategoryProject = new HashSet<CategoryProject>();
            ProjectImages = new HashSet<ProjectImage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public byte[] Icon { get; set; }
        public string IconMimeType { get; set; }
        public bool IsPublished { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual ICollection<CategoryProject> CategoryProject { get; set; }
        public virtual ICollection<ProjectImage> ProjectImages { get; set; }
    }
}
