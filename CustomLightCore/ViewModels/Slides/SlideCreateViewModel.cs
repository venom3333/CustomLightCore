using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CustomLightCore.ViewModels.Slides
{
    public class SlideCreateViewModel
    {
		[Required(ErrorMessage = "Выберите изображение!")]
		[DataType(DataType.Upload)]
		public IFormFile Image { get; set; }

		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.Text)]
		public string Description { get; set; }

		public static explicit operator Slide(SlideCreateViewModel item)
		{

			Slide result = new Slide
			{
				Name = item.Name,
				Description = item.Description,
			};

			// изображение
			if (item.Image != null && item.Image.ContentType.ToLower().StartsWith("image/"))
			{
				MemoryStream ms = new MemoryStream();
				item.Image.OpenReadStream().CopyTo(ms);

				result.ImageData = ms.ToArray();
				result.ImageMimeType = item.Image.ContentType;
			}
			return result;
		}

		public Slide GetModelByViewModel()
		{
			return (Slide)this;
		}
	}
}
