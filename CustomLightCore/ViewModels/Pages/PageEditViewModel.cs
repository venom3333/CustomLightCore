using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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

        [Required(ErrorMessage = "Укажите порядковый номер страницы!")]
        //[DataType(DataType.Text)]
        public string Weight { get; set; }

        [DataType(DataType.MultilineText)]
		public string PageContent { get; set; }

		/// <summary>
		/// Приведение экземпляра доменной модели во viewModel.
		/// </summary>
		public static explicit operator PageEditViewModel(Page item)
		{
			if(item == null)
			{
				return null;
			}

			PageEditViewModel result = new PageEditViewModel
			{
				Alias = item.Alias,
				Id = item.Id,
				Name = item.Name,
                Weight = item.Weight.ToString(),
				PageContent = item.PageContent
			};
			return result;
		}

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator Page(PageEditViewModel item)
		{
			Page result = new Page();
			using (CustomLightContext db = new CustomLightContext())
			{
				result = db.Pages.Find(item.Id);
			}

            result.Name = item.Name;
			result.Alias = item.Alias;
            result.Weight = int.TryParse(item.Weight, out int weight) ? weight : 0;
			result.PageContent = item.PageContent;
			result.Updated = DateTime.Now;

			return result;
		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Page GetModelByViewModel()
		{
			return (Page)this;
		}

		/// <summary>
		/// Получаем ВьюМодель на основе id ДатаМодели
		/// </summary>
		public static async Task<PageEditViewModel> GetViewModelByModelId(int? id)
		{
			if (id == null)
			{
				return null;
			}

			Page page = new Page();
			using (CustomLightContext db = new CustomLightContext())
			{
				page = await db.Pages.FindAsync(id);
			}
			return (PageEditViewModel)page;
		}
	}
}
