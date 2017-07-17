using CustomLightCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLightCore.ViewModels.Pages
{
    public class PageCreateViewModel : PageBaseViewModel
	{
		[Required(ErrorMessage = "Введите Алиас!")]
		[DataType(DataType.Text)]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Введите название страницы!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }
		
		[DataType(DataType.MultilineText)]
		public string PageContent { get; set; }

		///// <summary>
		///// Приведение экземпляра доменной модели во viewModel.
		///// </summary>
		//public static explicit operator PageCreateViewModel(Page item)
		//{
		//	if (item == null)
		//	{
		//		return null;
		//	}

		//	PageCreateViewModel result = new PageCreateViewModel
		//	{
		//		Alias = item.Alias,
		//		Name = item.Name,
		//		PageContent = item.PageContent
		//	};
		//	return result;
		//}

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Page(PageCreateViewModel item)
		{
			var now = DateTime.Now;

			Page result = new Page()
			{
				Name = item.Name,
				Alias = item.Alias,
				PageContent = item.PageContent,
				Created = now,
				Updated = now
			};
			return result;
		}
	}
}
