using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomLightCore.Models;

namespace CustomLightCore.ViewModels.ProductTypes
{
	public class ProductTypeCreateViewModel
	{
		[Required(ErrorMessage = "Введите наименование!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		//[Required]
		[DataType(DataType.Text)]
		public SpecificationTitle SpecificationTitle { get; set; }
		
		[Required(ErrorMessage = "Необходимо указать хотябы одно свойство продукта!")]
		public List<SpecificationTitle> SpecificationTitles { get; set; }
		

		/// <summary>
		/// Явное преобразование из вью-модели в доменную.
		/// </summary>
		public static explicit operator ProductType(ProductTypeCreateViewModel item)
		{
			var specificationTitles = new HashSet<SpecificationTitle>(item.SpecificationTitles);
			var result = new ProductType
			{
				Name = item.Name,
				SpecificationTitles = specificationTitles
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
