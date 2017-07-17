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

namespace CustomLightCore.Controllers
{
	public class CategoriesController : BaseController
	{
		//public CategoriesController()
		//{
		//}

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

			var categories = await db.Categories
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
		public async Task<IActionResult> Create([Bind("Id,Name,Description,ShortDescription")] Category categories, [Bind("Icon")] IFormFile icon)
		{
			if (ModelState.IsValid)
			{
				// Иконка
				if (icon != null && icon.ContentType.ToLower().StartsWith("image/"))
				{
					MemoryStream ms = new MemoryStream();
					await icon.OpenReadStream().CopyToAsync(ms);

					categories.Icon = ms.ToArray();
					categories.IconMimeType = icon.ContentType;
				}

				// Datetimes
				var now = DateTime.Now;
				categories.Created = now;
				categories.Updated = now;

				db.Add(categories);
				await db.SaveChangesAsync();
				return RedirectToAction("List");
			}

			await CreateViewBag();
			return View(categories);
		}

		// GET: Categories/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var categories = await db.Categories.SingleOrDefaultAsync(m => m.Id == id);
			if (categories == null)
			{
				return NotFound();
			}
			return View(categories);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ShortDescription")] Category newCategoryData, [Bind("NewIcon")] IFormFile newIcon)
		{
			if (id != newCategoryData.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				// Старые данные объекта
				Category oldCategoryData = await db.Categories.FindAsync(id);

				// Основные данные
				if (newCategoryData.Name != null)
				{
					oldCategoryData.Name = newCategoryData.Name;
					oldCategoryData.Description = newCategoryData.Description;
					oldCategoryData.ShortDescription = newCategoryData.ShortDescription;
				}

				// Иконка
				if (newIcon != null && newIcon.ContentType.ToLower().StartsWith("image/"))
				{
					MemoryStream ms = new MemoryStream();
					await newIcon.OpenReadStream().CopyToAsync(ms);

					oldCategoryData.Icon = ms.ToArray();
					oldCategoryData.IconMimeType = newIcon.ContentType;
				}

				// Datetimes
				var now = DateTime.Now;
				oldCategoryData.Updated = now;

				try
				{
					db.Update(oldCategoryData);
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
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var categories = await db.Categories
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
			var categories = await db.Categories.SingleOrDefaultAsync(m => m.Id == id);
			db.Categories.Remove(categories);
			await db.SaveChangesAsync();
			return RedirectToAction("List");
		}

		private bool CategoriesExists(int id)
		{
			return db.Categories.Any(e => e.Id == id);
		}

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetCategoryIcon(int? Id)
		{
			Category cat = db.Categories
				.FirstOrDefault(c => c.Id == Id);

			if (cat.Icon != null)
			{
				return File(cat.Icon, cat.IconMimeType);
			}
			else
			{
				return null;
			}
		}

	}
}
