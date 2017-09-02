using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CustomLightCore.ViewModels.Essentials
{
    public class EssentialEditViewModel : BaseViewModel
    {
		public int Id { get; set; }

		[Required(ErrorMessage = "Введите наименование компании!")]
		[DataType(DataType.Text)]
		public string Title { get; set; }

		[Required(ErrorMessage = "Введите информацию о компании!")]
		[DataType(DataType.MultilineText)]
		public string About { get; set; }

		[Required(ErrorMessage = "Введите адрес компании!")]
		[DataType(DataType.Text)]
		public string Address { get; set; }

		[Required(ErrorMessage = "Введите телефон!")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Введите имя директора!")]
		[DataType(DataType.PhoneNumber)]
		public string Boss { get; set; }

		[Required(ErrorMessage = "Введите email!")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile LogoImageData { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile LogoImageInvertedData { get; set; }


		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Essential(EssentialEditViewModel item)
		{
			Essential result = new Essential();
			using (CustomLightContext db = new CustomLightContext())
			{
				result = db.Essentials.FirstOrDefault();
			}

			if(result == null)
			{
				return null;
			}

			result.About = item.About;
			result.Address = item.Address;
			result.Boss = item.Boss;
			result.Email = item.Email;
			result.Id = item.Id;
			result.Phone = item.Phone;
			result.Title = item.Title;


			// иконка нормал
			if (item.LogoImageData != null && item.LogoImageData.ContentType.ToLower().StartsWith("image/"))
			{
				MemoryStream ms = new MemoryStream();
				item.LogoImageData.OpenReadStream().CopyTo(ms);

                // обработка изображения
                var processedImage = ImageProcess(ms.ToArray(), ImageType.Logo);

                result.LogoImageData = processedImage;
				result.LogoImageMimeType = item.LogoImageData.ContentType;
			}

			// иконка инвертед
			if (item.LogoImageInvertedData != null && item.LogoImageInvertedData.ContentType.ToLower().StartsWith("image/"))
			{
				MemoryStream ms = new MemoryStream();
				item.LogoImageInvertedData.OpenReadStream().CopyTo(ms);

                // обработка изображения
                var processedImage = ImageProcess(ms.ToArray(), ImageType.Logo);

                result.LogoImageInvertedData = processedImage;
                result.LogoImageInvertedMimeType = item.LogoImageInvertedData.ContentType;
			}

			return result;

		}

		/// <summary>
		/// Приведение экземпляра доменной модели во viewModel.
		/// </summary>
		public static explicit operator EssentialEditViewModel(Essential item)
		{
			if (item == null)
			{
				return null;
			}

			EssentialEditViewModel result = new EssentialEditViewModel
			{
				About = item.About,
				Address = item.Address,
				Boss = item.Boss,
				Email = item.Email,
				Id = item.Id,
				Phone = item.Phone,
				Title = item.Title
			};
			return result;
		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Essential GetModelByViewModel()
		{
			return (Essential)this;
		}

		/// <summary>
		/// Получаем ВьюМодель на основе id ДатаМодели
		/// </summary>
		public static async Task<EssentialEditViewModel> GetViewModelByModel()
		{
			Essential essential = new Essential();
			using (CustomLightContext db = new CustomLightContext())
			{
				essential = await db.Essentials.FirstOrDefaultAsync();
			}
			return (EssentialEditViewModel)essential;
		}
	}
}
