namespace CustomLightCore.ViewModels.Projects
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

    public class ProjectViewModel
    {
        public int Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Введите наименование!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Краткое описание")]
        [DataType(DataType.Text)]
        public string ShortDescription { get; set; }

        [DisplayName("Загрузить Иконку")]
        [DataType(DataType.Upload)]
        public IFormFile Icon { get; set; }

        [DisplayName("Опубликовано")]
        public bool IsPublished { get; set; }

        // Категории
        [DisplayName("Категории")]
        public List<int> CategoryProjectId { get; set; }

        // Изображения
        [DisplayName("Добавить Изображения")]
        public virtual List<IFormFile> ProjectImages { get; set; }

        /// <summary>
        /// Gets or sets the existing project image ids.
        /// </summary>
        [DisplayName("Текущие изображения")]
        public List<int> ExistingProjectImageIds { get; set; }

        /// <summary>
        /// Явное преобразование из вью-модели в доменную.
        /// </summary>
        public static explicit operator Project(ProjectViewModel item)
        {
            var now = DateTime.Now;

            var result = new Project();

            if (item.Id != 0)
            {
                using (var db = new CustomLightContext())
                {
                    result = db.Projects
                        .Include(p => p.CategoryProject)
                        .Include(p => p.ProjectImages)
                        .FirstOrDefault(p => p.Id == item.Id);

                    // Удаленные категории удаляем из контекста
                    if (result.CategoryProject != null)
                    {
                        var categoriesToRemove = new List<CategoryProject>();
                        foreach (var categoryProject in result.CategoryProject)
                        {
                            if (!item.CategoryProjectId.Contains(categoryProject.CategoriesId))
                            {
                                categoriesToRemove.Add(categoryProject);
                            }
                        }
                        db.CategoryProject.RemoveRange(categoriesToRemove);
                        db.SaveChanges();
                    }

                    // Удаленные изображения удаляем из контекста
                    if (result.ProjectImages != null)
                    {
                        if (item.ExistingProjectImageIds != null)
                        {
                            var imagesToRemove = new List<ProjectImage>();
                            foreach (var projectImage in result.ProjectImages)
                            {
                                if (!item.ExistingProjectImageIds.Contains(projectImage.Id))
                                {
                                    imagesToRemove.Add(projectImage);
                                }
                            }
                            db.ProjectImages.RemoveRange(imagesToRemove);
                            db.SaveChanges();
                        }
                    }
                }
            }

            result.Name = item.Name;
            result.Description = item.Description;
            result.ShortDescription = item.ShortDescription;
            result.Updated = now;
            result.IsPublished = item.IsPublished;


            // Категории проекта
            if (item.CategoryProjectId != null)
            {
                var categoryProjects = new HashSet<CategoryProject>();
                foreach (var categoryId in item.CategoryProjectId)
                {
                    if (result.CategoryProject.Select(cp => cp.CategoriesId).ToList().Contains(categoryId))
                    {
                        continue;
                    }
                    var categoryProject = new CategoryProject
                    {
                        CategoriesId = categoryId
                    };
                    categoryProjects.Add(categoryProject);
                }
                result.CategoryProject = categoryProjects;
            }

            // иконка
            if (item.Icon != null && item.Icon.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream ms = new MemoryStream();
                item.Icon.OpenReadStream().CopyTo(ms);

                result.Icon = ms.ToArray();
                result.IconMimeType = item.Icon.ContentType;
            }

            // изображения
            if (item.ProjectImages != null)
            {
                var projectImages = new HashSet<ProjectImage>();
                foreach (var projectImage in item.ProjectImages)
                {
                    if (projectImage != null && projectImage.ContentType.ToLower().StartsWith("image/"))
                    {
                        var ms = new MemoryStream();
                        projectImage.OpenReadStream().CopyTo(ms);

                        var image = new ProjectImage
                        {
                            ImageData = ms.ToArray(),
                            ImageMimeType = projectImage.ContentType
                        };
                        projectImages.Add(image);
                    }
                }
                // Теперь добавим то, что осталось в ExistingProjectImagesIds
                if (item.ExistingProjectImageIds != null)
                {
                    foreach (var imageId in item.ExistingProjectImageIds)
                    {
                        using (var db = new CustomLightContext())
                        {
                            var image = db.ProjectImages.Find(imageId);
                            projectImages.Add(image);
                        }
                    }
                }
                result.ProjectImages = projectImages;
            }


            return result;
        }

        /// <summary>
        /// Получаем ДатаМодель на основе существующей вью модели
        /// </summary>
        public Project GetModelByViewModel()
        {
            return (Project)this;
        }

        /// <summary>
        /// Приведение экземпляра доменной модели во viewModel.
        /// </summary>
        public static explicit operator ProjectViewModel(Project item)
        {
            if (item == null)
            {
                return null;
            }

            ProjectViewModel result = new ProjectViewModel
            {
                Id = item.Id,
                Description = item.Description,
                ShortDescription = item.ShortDescription,
                Name = item.Name,
                IsPublished = item.IsPublished,
                ExistingProjectImageIds = item.ProjectImages.Select(image => image.Id).ToList(),
                CategoryProjectId = item.CategoryProject.Select(cp => cp.CategoriesId).ToList()
            };
            return result;
        }

        /// <summary>
        /// Получаем ВьюМодель на основе id ДатаМодели
        /// </summary>
        public static async Task<ProjectViewModel> GetViewModelByModelId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var project = new Project();
            using (var db = new CustomLightContext())
            {
                project = await db.Projects
                    .Include(p => p.CategoryProject)
                    .Include(p => p.ProjectImages)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            return (ProjectViewModel)project;
        }
    }
}