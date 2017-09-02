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
	public class CategoryCreateViewModel : BaseViewModel
    {
		[Required(ErrorMessage = "Введите наименование!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[DataType(DataType.Text)]
		public string ShortDescription { get; set; }

		[Required(ErrorMessage = "Выберите иконку!")]
		[DataType(DataType.Upload)]
		public IFormFile Icon { get; set; }

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Category(CategoryCreateViewModel item)
		{
			var now = DateTime.Now;

			Category result = new Category()
			{
				Name = item.Name,
				Description = item.Description,
				ShortDescription = item.ShortDescription,
				Created = now,
				Updated = now
			};

			// иконка
			if (item.Icon != null && item.Icon.ContentType.ToLower().StartsWith("image/"))
			{
				MemoryStream ms = new MemoryStream();
				item.Icon.OpenReadStream().CopyTo(ms);

                // обработка изображения
                var processedImage = ImageProcess(ms.ToArray(), ImageType.Icon);

                result.Icon = processedImage;
				result.IconMimeType = item.Icon.ContentType;
			}

			return result;

		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Category GetModelByViewModel()
		{
			return (Category)this;
		}
	}
}
