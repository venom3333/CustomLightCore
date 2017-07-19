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
	public class SlideEditViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Выберите изображение!")]
		[DataType(DataType.Upload)]
		public IFormFile Image { get; set; }

		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.Text)]
		public string Description { get; set; }

		public static explicit operator Slide(SlideEditViewModel item)
		{

			Slide result = new Slide();
			using (CustomLightContext db = new CustomLightContext())
			{
				result = db.Slides.Find(item.Id);
			}

			result.Name = item.Name;
			result.Description = item.Description;


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

		public static explicit operator SlideEditViewModel(Slide item)
		{
			if (item == null)
			{
				return null;
			}

			SlideEditViewModel slide = new SlideEditViewModel
			{
				Id = item.Id,
				Description = item.Description,
				Name = item.Name
			};

			return slide;
		}

		public static async Task<SlideEditViewModel> GetViewModelByModelId(int? id)
		{
			if (id == null)
			{
				return null;
			}

			Slide slide = new Slide();

			using(CustomLightContext db = new CustomLightContext())
			{
				slide = await db.Slides.FindAsync(id);
			}

			return (SlideEditViewModel)slide;
		}
	}
}
