using CustomLightCore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLightCore.ViewModels.Products
{
    public class ProductViewModel
    {
		public int Id { get; set; }

		[Required(ErrorMessage = "Введите наименование!")]
		[DataType(DataType.Text)]
		public string Name { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		[DataType(DataType.Text)]
		public string ShortDescription { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile Icon { get; set; }

		public bool IsPublished { get; set; }

		// Тип продукта
		public int ProductTypeId { get; set; }

		// Категории
		public List<int> CategoryProductId { get; set; }

		// Изображения
		public virtual List<IFormFile> ProductImages { get; set; }

		// Спецификации
		public Specification Specification { get; set; }
		public List<Specification> Specifications { get; set; }

		public ProductType ProductType { get; set; }

		//public ProductViewModel()
		//{
		//	CategoryProduct = new HashSet<CategoryProduct>();
		//	ProductImages = new HashSet<ProductImage>();
		//	Specifications = new HashSet<Specification>();
		//}
	}
}
