using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using CustomLightCore.ViewModels.Categories;
using CustomLightCore.ViewModels.ProductTypes;
using Microsoft.AspNetCore.Authorization;

namespace CustomLightCore.Controllers
{
	public class ProductTypesController : BaseController
	{
		// GET
		[Authorize]
		public async Task<IActionResult> List()
		{
			var productTypes = await db.ProductTypes.ToListAsync();

			return View(productTypes);
		}

		// GET: ProductTypes/Create
		[Authorize]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		// POST: ProductTypes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create(
			[Bind("Name,SpecificationTitles")] ProductTypeCreateViewModel createdProductType)
		{
			if (ModelState.IsValid)
			{
				var productType = createdProductType.GetModelByViewModel();

				db.Add(productType);
				await db.SaveChangesAsync();
				return RedirectToAction("List");
			}

			return View(createdProductType);
		}

		// GET: ProductTypes/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productTypeEditViewModel = await ProductTypeEditViewModel.GetViewModelByModelId(id);
			if (productTypeEditViewModel == null)
			{
				return NotFound();
			}
			return View(productTypeEditViewModel);
		}

		// POST: ProductTypes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id,
			[Bind("Id,Name,SpecificationTitles")] ProductTypeEditViewModel newProductTypeData)
		{
			if (id != newProductTypeData.Id)
			{
				return NotFound();
			}
			
			if (ModelState.IsValid)
			{
				try
				{
					// Удаляем из контекста уже не нужные SpecificationTitles
					var oldSpecificationTitlesId = db.ProductTypes.Include(pt => pt.SpecificationTitles).AsNoTracking()
							.FirstOrDefault(pt => pt.Id == newProductTypeData.Id)
							.SpecificationTitles.Select(i => i.Id);

					var newSpecificationTitlesId = newProductTypeData.SpecificationTitles.Select(i => i.Id);

					var remove = oldSpecificationTitlesId.Except(newSpecificationTitlesId);

					db.SpecificationTitles.RemoveRange(db.SpecificationTitles.Where(i => remove.Contains(i.Id)));

					// Старые данные объекта
					ProductType newProductType = newProductTypeData.GetModelByViewModel();

					db.Update(newProductType);
					await db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductTypeExists(newProductTypeData.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("List");
			}
			return View(newProductTypeData);
		}


		// GET: ProductTypes/Delete/5
		// TODO: Предусмотреть предупреждение о невозможности удалить категорию если к ней привязаны продукты/проекты
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productType = await db.ProductTypes
				.Include(pt => pt.SpecificationTitles)
				.SingleOrDefaultAsync(m => m.Id == id);
			if (productType == null)
			{
				return NotFound();
			}

			await CreateViewBag();
			return View(productType);
		}

		// POST: ProductTypes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var productType = db.ProductTypes.SingleOrDefault(pt => pt.Id == id);
			db.ProductTypes.Remove(productType);
			await db.SaveChangesAsync();
			return RedirectToAction("List");
		}

		private bool ProductTypeExists(int id) => db.ProductTypes.Any(e => e.Id == id);

		// Добавить SpecificationTitle для Create
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult GenerateSpecificationTitleCreate(ProductTypeCreateViewModel productType)
		{
			var specificationTitle = new SpecificationTitle();

			if (productType.SpecificationTitles == null)
			{
				productType.SpecificationTitles = new List<SpecificationTitle>();
			}

			productType.SpecificationTitles.Add(specificationTitle);

			return PartialView("_SpecificationTitlesCreate", productType);
		}

		// Убрать SpecificationTitle для Create
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveSpecificationTitleCreate(ProductTypeCreateViewModel productType,
			int specificationTitleIndex)
		{
			productType.SpecificationTitles.RemoveAt(specificationTitleIndex);

			ModelState.Clear();
			return PartialView("_SpecificationTitlesCreate", productType);
		}

		// Добавить SpecificationTitle для Edit
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult GenerateSpecificationTitleEdit(ProductTypeEditViewModel productType)
		{
			var specificationTitle = new SpecificationTitle();

			if (productType.SpecificationTitles == null)
			{
				productType.SpecificationTitles = new List<SpecificationTitle>();
			}

			productType.SpecificationTitles.Add(specificationTitle);

			return PartialView("_SpecificationTitlesEdit", productType);
		}

		// Убрать SpecificationTitle для Edit
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveSpecificationTitleEdit(ProductTypeEditViewModel productType,
			int specificationTitleIndex)
		{
			productType.SpecificationTitles.RemoveAt(specificationTitleIndex);

			ModelState.Clear();
			return PartialView("_SpecificationTitlesEdit", productType);
		}
	}
}