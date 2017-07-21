using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CustomLightCore.ViewModels.ProductTypes
{
	public class ProductTypeCreateViewModel
	{
		[Required(ErrorMessage = "Введите наименование!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public SpecificationTitle SpecificationTitle { get; set; }
		
		[Required(ErrorMessage = "Необходимо указать хотябы одно свойство продукта!")]
		public List<SpecificationTitle> SpecificationTitles { get; set; }
		

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator ProductType(ProductTypeCreateViewModel item)
		{
			ProductType result = new ProductType
			{
				Name = item.Name,
				SpecificationTitles = item.SpecificationTitles
			};

			return result;

		}

		/// <summary>
		/// Получаем ДатаМодель на основе существующей вью модели
		/// </summary>
		public ProductType GetModelByViewModel()
		{
			return (ProductType)this;
		}
	}
}
