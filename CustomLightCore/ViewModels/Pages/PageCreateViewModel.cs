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
		
		[DataType(DataType.MultilineText)]
		public string PageContent { get; set; }
	}
}
