using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomLightCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomLightCore.ViewModels.ProductTypes
{
    public class ProductTypeEditViewModel
    {
        public int Id { get; set; }

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
        public static explicit operator ProductType(ProductTypeEditViewModel item)
        {
            var specificationTitles = new HashSet<SpecificationTitle>(item.SpecificationTitles);
			var result = new ProductType();
			using (CustomLightContext ctx = new CustomLightContext())
			{
				result = ctx.ProductTypes
					.Include(pt => pt.SpecificationTitles)
					.FirstOrDefault(pt => pt.Id == item.Id);

				result.Name = item.Name;
				result.SpecificationTitles = specificationTitles;
				
			}
            return result;
        }

        /// <summary>
        /// Получаем ДатаМодель на основе существующей вью модели
        /// </summary>
        public ProductType GetModelByViewModel()
        {
            return (ProductType) this;
        }


        /// <summary>
        /// Приведение экземпляра доменной модели во viewModel.
        /// </summary>
        public static explicit operator ProductTypeEditViewModel(ProductType item)
        {
            if (item == null)
            {
                return null;
            }

            var result = new ProductTypeEditViewModel
            {
                Id = item.Id,
                Name = item.Name,
                SpecificationTitles = item.SpecificationTitles.ToList()
            };
            return result;
        }


        /// <summary>
        /// Получаем ВьюМодель на основе id ДатаМодели
        /// </summary>
        public static async Task<ProductTypeEditViewModel> GetViewModelByModelId(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var productType = new ProductType();
            using (var db = new CustomLightContext())
            {
                productType = await db.ProductTypes
                    .Include(pt => pt.SpecificationTitles)
                    .FirstOrDefaultAsync(pt => pt.Id == id);
            }
            return (ProductTypeEditViewModel) productType;
        }
    }
}