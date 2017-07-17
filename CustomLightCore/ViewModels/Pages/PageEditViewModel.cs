using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;

namespace CustomLightCore.ViewModels.Pages
{
	public class PageEditViewModel
	{

		public int Id { get; set; }

		[Required(ErrorMessage = "Введите Алиас!")]
		[DataType(DataType.Text)]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Введите название страницы!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		public string PageContent { get; set; }

		/// <summary>
		/// Приведение экземпляра доменной модели во viewModel.
		/// </summary>
		public static explicit operator PageEditViewModel(Page item)
		{
			return new PageEditViewModel();
		}

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Page(PageEditViewModel item)
		{
			return new Page();
		}
	}
}
