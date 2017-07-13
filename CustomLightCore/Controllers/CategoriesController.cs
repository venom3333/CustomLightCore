using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomLightCore.Controllers
{
    public class CategoriesController : BaseController
    {
		// GET: Categories
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public async Task<IActionResult> Index()
        {
			ViewBag.Categories = await db.Categories.ToListAsync();
			ViewBag.Projects = await db.Projects.ToListAsync();
			ViewBag.Pages = await db.Pages.ToListAsync();
			ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync();
			return View(await db.Categories.ToListAsync());
        }

		[Authorize]
		public async Task<IActionResult> List()
		{
			return View(await db.Categories.ToListAsync());
		}

		// GET: Categories/Details/5
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
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

			ViewBag.Categories = await db.Categories.ToListAsync();
			ViewBag.Projects = await db.Projects.ToListAsync();
			ViewBag.Pages = await db.Pages.ToListAsync();
			ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync();
			return View(categories);
        }

		// GET: Categories/Create
		public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,Created,Updated")] Category categories)
        {
            if (ModelState.IsValid)
            {
                db.Add(categories);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

		// GET: Categories/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,Created,Updated")] Category categories)
        {
            if (id != categories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(categories);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(categories);
        }

		// GET: Categories/Delete/5
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

            return View(categories);
        }

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await db.Categories.SingleOrDefaultAsync(m => m.Id == id);
            db.Categories.Remove(categories);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoriesExists(int id)
        {
            return db.Categories.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
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
