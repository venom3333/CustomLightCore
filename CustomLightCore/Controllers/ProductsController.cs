using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomLightCore.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Products
        [Authorize]
        public async Task<IActionResult> List()
        {
            var products = await db.Products
                .Include(p => p.ProductType)
                .Include(p => p.CategoryProduct)
                .ThenInclude(cp => cp.Categories)
                .ToListAsync();

            return View(products);
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
                .ThenInclude(s => s.SpecificationValues)
                .Include(p => p.ProductType)
                .ThenInclude(pt => pt.SpecificationTitles)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            await CreateViewBag();
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
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated,ProductTypeId")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
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
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated,ProductTypeId")]
            Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.ProductType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.SingleOrDefaultAsync(m => m.Id == id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }

        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
        public FileContentResult GetProductIcon(int? id)
        {
            Product prods = db.Products
                .FirstOrDefault(p => p.Id == id);

            if (prods.Icon != null)
            {
                return File(prods.Icon, prods.IconMimeType);
            }
            else
            {
                return null;
            }
        }

        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
        public FileContentResult GetProductImage(int? imageId)
        {
            ProductImage image = db.ProductImages.FirstOrDefault(i => i.Id == imageId);

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