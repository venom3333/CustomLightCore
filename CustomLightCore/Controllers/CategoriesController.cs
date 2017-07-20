using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using CustomLightCore.ViewModels.Categories;

namespace CustomLightCore.Controllers
{
	public class CategoriesController : BaseController
	{
		// GET: Categories
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public async Task<IActionResult> Index()
		{
			await CreateViewBag();
			return View(await db.Categories.ToListAsync());
		}

		[Authorize]
		public async Task<IActionResult> List()
		{
			return View(await db.Categories.ToListAsync());
		}

		// GET: Categories/Details/5
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Category categories = await db.Categories
				.Include(cProd => cProd.CategoryProduct)
					.ThenInclude(prod => prod.Products)
				.Include(cProj => cProj.CategoryProject)
					.ThenInclude(proj => proj.Projects)
				.SingleOrDefaultAsync(m => m.Id == id);
			if (categories == null)
			{
				return NotFound();
			}

			await CreateViewBag();
			return View(categories);
		}
		// GET: Categories/Create
		[Authorize]
		public async Task<IActionResult> Create()
		{
			await CreateViewBag();
			return View();
		}

		// POST: Category/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("Name,Description,ShortDescription,Icon")] CategoryCreateViewModel createdCategory)
		{
			if (ModelState.IsValid)
			{
				Category category = createdCategory.GetModelByViewModel();

				db.Add(category);
				await db.SaveChangesAsync();
				return RedirectToAction("List");
			}

			await CreateViewBag();
			return View(createdCategory);
		}
		// GET: Categories/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			CategoryEditViewModel category = await CategoryEditViewModel.GetViewModelByModelId(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ShortDescription,Icon")] CategoryEditViewModel newCategoryData)
		{
			if (id != newCategoryData.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				// Старые данные объекта
				Category category = newCategoryData.GetModelByViewModel();

				try
				{
					db.Update(category);
					await db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoriesExists(newCategoryData.Id))
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
			return View(newCategoryData);
		}

		// GET: Categories/Delete/5
		// TODO: Предусмотреть предупреждение о невозможности удалить категорию если к ней привязаны продукты/проекты
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Category categories = await db.Categories
				.SingleOrDefaultAsync(m => m.Id == id);
			if (categories == null)
			{
				return NotFound();
			}

			await CreateViewBag();
			return View(categories);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			Category categories = await db.Categories.SingleOrDefaultAsync(m => m.Id == id);
			db.Categories.Remove(categories);
			await db.SaveChangesAsync();
			return RedirectToAction("List");
		}

		private bool CategoriesExists(int id) => db.Categories.Any(e => e.Id == id);

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetCategoryIcon(int? id)
		{
			Category cat = db.Categories
				.FirstOrDefault(c => c.Id == id);

			return cat.Icon != null ? File(cat.Icon, cat.IconMimeType) : null;
		}

	}
}
