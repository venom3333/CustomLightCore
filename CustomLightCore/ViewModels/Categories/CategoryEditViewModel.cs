using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CustomLightCore.ViewModels.Categories
{
	public class CategoryEditViewModel
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

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Category(CategoryEditViewModel item)
		{
			Category result = new Category();
			using (CustomLightContext db = new CustomLightContext())
			{
				result = db.Categories.Find(item.Id);
			}

			var now = DateTime.Now;

			result.Name = item.Name;
			result.Description = item.Description;
			result.ShortDescription = item.ShortDescription;
			result.Updated = now;

			// иконка
			if (item.Icon != null && item.Icon.ContentType.ToLower().StartsWith("image/"))
			{
				MemoryStream ms = new MemoryStream();
				item.Icon.OpenReadStream().CopyTo(ms);

				result.Icon = ms.ToArray();
				result.IconMimeType = item.Icon.ContentType;
			}

			return result;

		}

		/// <summary>
		/// Приведение экземпляра доменной модели во viewModel.
		/// </summary>
		public static explicit operator CategoryEditViewModel(Category item)
		{
			if (item == null)
			{
				return null;
			}

			CategoryEditViewModel result = new CategoryEditViewModel
			{
				Id = item.Id,
				Description = item.Description,
				ShortDescription = item.ShortDescription,
				Name = item.Name,
			};
			return result;
		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Category GetModelByViewModel()
		{
			return (Category)this;
		}

		/// <summary>
		/// Получаем ВьюМодель на основе id ДатаМодели
		/// </summary>
		public static async Task<CategoryEditViewModel> GetViewModelByModelId(int? id)
		{
			if (id == null)
			{
				return null;
			}

			Category category = new Category();
			using (CustomLightContext db = new CustomLightContext())
			{
				category = await db.Categories.FindAsync(id);
			}
			return (CategoryEditViewModel)category;
		}
	}
}
