using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;

namespace CustomLightCore.Controllers
{
    public class CategoryController : BaseController
    {
		// GET: Category
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public async Task<IActionResult> Index()
        {
			ViewBag.Categories = await _context.Categories.ToListAsync();
			ViewBag.Projects = await _context.Projects.ToListAsync();
			ViewBag.Pages = await _context.Pages.ToListAsync();
			ViewBag.Essentials = await _context.Essentials.FirstOrDefaultAsync(e => e != null);
			return View(await _context.Categories.ToListAsync());
        }

		/*
		 public async Task<IActionResult> Index()
{
    var courses = _context.Courses
        .Include(c => c.Department)
        .AsNoTracking();
    return View(await courses.ToListAsync());
} 
ctx.EntityOne
    .Include(eOne => eOne.EntityTwo)
    .ThenInclude(eTwo => eTwo.SomeOtherEntity)
    .Where(entityOne => YourQuery);
		  
		 */

		// GET: Category/Details/5
		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
				.Include(cProd => cProd.CategoryProduct)
					.ThenInclude(prod => prod.Products)
				.Include(cProj => cProj.CategoryProject)
					.ThenInclude(proj => proj.Projects)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

			ViewBag.Categories = await _context.Categories.ToListAsync();
			ViewBag.Projects = await _context.Projects.ToListAsync();
			ViewBag.Pages = await _context.Pages.ToListAsync();
			ViewBag.Essentials = await _context.Essentials.FirstOrDefaultAsync(e => e != null);
			return View(categories);
        }

        // GET: Category/Create
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
                _context.Add(categories);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.SingleOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Category/Edit/5
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
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
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

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.SingleOrDefaultAsync(m => m.Id == id);
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public FileContentResult GetCategoryIcon(int? Id)
		{
			Category cat = _context.Categories
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
