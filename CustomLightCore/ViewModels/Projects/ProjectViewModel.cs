using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CustomLightCore.Models;

namespace CustomLightCore.ViewModels.Projects
{
	public class ProjectViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Введите наименование!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[DataType(DataType.Text)]
		public string ShortDescription { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile Icon { get; set; }

		public bool IsPublished { get; set; }

		// Категории
		public List<int> CategoryProjectId { get; set; }

		// Изображения
		public virtual List<IFormFile> ProjectImages { get; set; }


		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Project(ProjectViewModel item)
		{
			var now = DateTime.Now;

			var result = new Project
			{
				Name = item.Name,
				Description = item.Description,
				ShortDescription = item.ShortDescription,
				Created = now,
				Updated = now,
				IsPublished = item.IsPublished

			};
			
			// Категории проекта
			if (item.CategoryProjectId != null)
			{
				var categoryProjects = new HashSet<CategoryProject>();
				foreach (var categoryId in item.CategoryProjectId)
				{
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
			result.ProjectImages = projectImages;

			return result;

		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Project GetModelByViewModel()
		{
			return (Project)this;
		}
		//public ProjectViewModel()
		//{
		//	CategoryProject = new HashSet<CategoryProject>();
		//	ProjectImages = new HashSet<ProjectImage>();
		//	Specifications = new HashSet<Specification>();
		//}
	}
}