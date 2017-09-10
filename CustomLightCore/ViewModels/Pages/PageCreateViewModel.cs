using CustomLightCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLightCore.ViewModels.Pages
{
    public class PageCreateViewModel
	{
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
                Weight = int.TryParse(item.Weight, out int weight) ? weight : 0,
				Created = now,
				Updated = now
			};
			return result;
		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public Page GetModelByViewModel()
		{
			return (Page)this;
		}

	}
}
