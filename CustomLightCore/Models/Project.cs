using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomLightCore.Models
{
    public sealed partial class Project
    {
        public Project()
        {
            CategoryProject = new HashSet<CategoryProject>();
            ProjectImages = new HashSet<ProjectImage>();
        }

        public int Id { get; set; }
        
        [DisplayName("Наименование")]
        public string Name { get; set; }
        
        [DisplayName("Описание")]
        public string Description { get; set; }
        
        [DisplayName("Краткое описание")]
        public string ShortDescription { get; set; }
        
        [DisplayName("Иконка")]
        public byte[] Icon { get; set; }
        public string IconMimeType { get; set; }
        
        [DisplayName("Опубликовано")]
        public bool IsPublished { get; set; }
        
        [DisplayName("Создано")]
        //public DateTime? Created { get; set; }
        public DateTime Created {
            get => _created ?? DateTime.Now;

            set => _created = value;
        }
        
        private DateTime? _created;
        
        [DisplayName("Обновлено")]
        public DateTime Updated { get; set; }

        public ICollection<CategoryProject> CategoryProject { get; set; }
        public ICollection<ProjectImage> ProjectImages { get; set; }
    }
}
