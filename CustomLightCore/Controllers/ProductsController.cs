// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsController.cs" company="CustomLight">
//   Venom
// </copyright>
// <summary>
//   The products controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomLightCore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.Models;
    using CustomLightCore.ViewModels.Products;
    using CustomLightCore.ViewModels.Specifications;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The products controller.
    /// </summary>
    public class ProductsController : BaseController
    {
        /// <summary>
        /// GET: Products
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        public async Task<IActionResult> List()
        {
            var products = await db.Products
                .Include(p => p.ProductType)
                .Include(p => p.CategoryProduct)
                .ThenInclude(cp => cp.Categories)
                .ToListAsync();

            return View("~/Views/Products/List.cshtml",products);
        }

        /// <summary>
        /// GET: Products/Details/5
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
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

        /// <summary>
        /// GET: Products/Create
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name");
            ViewData["Categories"] = new SelectList(db.Categories, "Id", "Name");
            return View("~/Views/Products/Create.cshtml");
        }

        /// <summary>
        /// POST: Products/Create
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,Name,Description,ShortDescription,Icon,IsPublished,ProductTypeId,CategoryProductId,ProductImages,Specifications")]
            ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }

            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", product.ProductTypeId);
            ViewData["Categories"] = new SelectList(db.Categories, "Id", "Name", product.CategoryProductId);

            return View("~/Views/Products/Create.cshtml", product);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// // GET: Products/Edit/5
        [Authorize]
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

        /// <summary>
        /// POST: Products/Edit/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(
            int id,
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

                    throw;
                }

                return RedirectToAction("List");
            }

            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        /// <summary>
        /// GET: Products/Delete/5
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
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

        /// <summary>
        /// POST: Products/Delete/5
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.SingleOrDefaultAsync(m => m.Id == id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        /// <summary>
        /// The get product icon.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="FileContentResult"/>.
        /// </returns>
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
        public FileContentResult GetProductIcon(int? id)
        {
            var prods = db.Products
                .FirstOrDefault(p => p.Id == id);

            return prods.Icon != null ? this.File(prods.Icon, prods.IconMimeType) : null;
        }

        /// <summary>
        /// The get product image.
        /// </summary>
        /// <param name="imageId">
        /// The image id.
        /// </param>
        /// <returns>
        /// The <see cref="FileContentResult"/>.
        /// </returns>
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
        public FileContentResult GetProductImage(int? imageId)
        {
            var image = db.ProductImages.FirstOrDefault(i => i.Id == imageId);

            return image != null ? File(image.ImageData, image.ImageMimeType) : null;
        }

        /// <summary>
        ///  Добавить спецификацию
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateSpecification(
            [Bind(
                "Id,Name,Description,ShortDescription,Icon,IsPublished,ProductTypeId,CategoryProductId,ProductImages,Specifications")]
            ProductViewModel product)
        {
            var specification = new SpecificationViewModel();
            if (product.Specifications == null)
            {
                product.Specifications = new List<SpecificationViewModel>();
            }

            product.Specifications.Add(specification);

            product = FillViewModelSpecifications(product);

            return PartialView("_SpecificationsCreate", product);
        }

        /// <summary>
        /// Убрать спецификацию
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <param name="specificationIndex">
        /// The specification index.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSpecification(
            [Bind("Id,Name,Description,ShortDescription,Icon,IsPublished,ProductTypeId,CategoryProductId,ProductImages,Specifications")]
            ProductViewModel product,
            int specificationIndex)
        {
            // Добираем тип продукта для того чтобы знать как отображать спецификации
            product.ProductType = db.ProductTypes.Include(pt => pt.SpecificationTitles)
                .AsNoTracking()
                .FirstOrDefault(pt => pt.Id == product.ProductTypeId);

            product.Specifications.RemoveAt(specificationIndex);

            ModelState.Clear();

            product = FillViewModelSpecifications(product);

            return PartialView("_SpecificationsCreate", product);
        }

        /// <summary>
        /// Обновить область отображения спецификаций
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSpecifications(ProductViewModel product)
        {
            // Добираем тип продукта для того чтобы знать как отображать спецификации
            product.ProductType = db.ProductTypes.Include(pt => pt.SpecificationTitles)
                .AsNoTracking()
                .FirstOrDefault(pt => pt.Id == product.ProductTypeId);

            product.Specifications = new List<SpecificationViewModel>();

            ModelState.Clear();

            product = FillViewModelSpecifications(product);

            return PartialView("_SpecificationsCreate", product);
        }

        /// <summary>
        /// Вижу / Не вижу
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> TogglePublish([Bind("id")]int? id)
        {
            if (id == null)
            {
                return false;
            }

            var product = await db.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return false;
            }

            product.IsPublished = !product.IsPublished;
            db.Update(product);
            await db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// The products exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ProductsExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }

        /// <summary>
        /// The fill view model specifications.
        /// </summary>
        /// <param name="productViewModel">
        /// The product view model.
        /// </param>
        /// <returns>
        /// The <see cref="ProductViewModel"/>.
        /// </returns>
        private ProductViewModel FillViewModelSpecifications(ProductViewModel productViewModel)
        {
            // Добираем тип продукта для того чтобы знать как отображать спецификации
            productViewModel.ProductType = db.ProductTypes
                .Include(pt => pt.SpecificationTitles)
                .AsNoTracking()
                .FirstOrDefault(pt => pt.Id == productViewModel.ProductTypeId);

            // Наполним пустыми Values если не заполненно
            productViewModel.Specifications.ForEach(spec =>
                {
                    if (spec.SpecificationValues == null)
                    {
                        spec.SpecificationValues = new List<SpecificationValue>();
                    }

                    if (spec.SpecificationValues.Count == 0)
                    {
                        foreach (var item in productViewModel.ProductType.SpecificationTitles)
                        {
                            spec.SpecificationValues.Add(
                                new SpecificationValue
                                {
                                    Value = string.Empty,
                                    Specification = spec,
                                    SpecificationId = spec.Id,
                                    SpecificationTitle = item,
                                    SpecificationTitleId = item.Id
                                });
                        }
                    }
                });

            return productViewModel;
        }
    }
}