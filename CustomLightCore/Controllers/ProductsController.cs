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
    public class ProductsController : BaseController
    {
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var customLightContext = db.Products.Include(p => p.ProductType);
            return View(await customLightContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await db.Products
				.Include(p => p.ProductImages)
				.Include(p => p.Specifications)
					.ThenInclude(s=>s.SpecificationValues)
                .Include(p => p.ProductType)
					.ThenInclude(pt=>pt.SpecificationTitles)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

			ViewBag.Categories = await db.Categories.ToListAsync();
			ViewBag.Projects = await db.Projects.ToListAsync();
			ViewBag.Pages = await db.Pages.ToListAsync();
			ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync(e => e != null);
			return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated,ProductTypeId")] Product products)
        {
            if (ModelState.IsValid)
            {
                db.Add(products);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", products.ProductTypeId);
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await db.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", products.ProductTypeId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated,ProductTypeId")] Product products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(products);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", products.ProductTypeId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await db.Products
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await db.Products.SingleOrDefaultAsync(m => m.Id == id);
            db.Products.Remove(products);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public FileContentResult GetProductIcon(int? Id)
		{
			Product prods = db.Products
				.FirstOrDefault(p => p.Id == Id);

			if (prods.Icon != null)
			{
				return File(prods.Icon, prods.IconMimeType);
			}
			else
			{
				return null;
			}
		}

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public FileContentResult GetProductImage(int? ImageId)
		{
			ProductImage image = db.ProductImages.FirstOrDefault(i => i.Id == ImageId);

			if (image != null)
			{
				return File(image.ImageData, image.ImageMimeType);
			}
			else
			{
				return null;
			}
		}
	}
}