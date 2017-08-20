namespace CustomLightCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.Models;
    using CustomLightCore.ViewModels.Cart;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The cart controller.
    /// </summary>
    public class CartController : BaseController
    {
        /// <summary>
        /// The details.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> Details()
        {
            await CreateViewBag();
            return View(Cart);
        }

        /// <summary>
        /// The add to cart.
        /// </summary>
        /// <param name="specificationId">
        /// The specification Id.
        /// </param>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        public IActionResult AddToCart(int specificationId, int quantity)
        {
            if (quantity == 0)
            {
                return this.PartialView("~/Views/Shared/_CartIcon.cshtml", Cart);
            }

            var specification = this.db.Specifications
                .Include(sp => sp.Product)
                    .ThenInclude(p => p.ProductType)
                .Include(sp => sp.SpecificationValues)
                    .ThenInclude(sv => sv.SpecificationTitle)
                .FirstOrDefault(sp => sp.Id == specificationId);

            if (specification != null)
            {
                Cart = Cart.AddToCart(specification, quantity);
            }
            return PartialView("~/Views/Shared/_CartIcon.cshtml", Cart);
        }

        /// <summary>
        /// The remove from cart.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IActionResult RemoveFromCart(int id)
        {
            Cart = Cart.RemoveSpecification(id);
            return RedirectToAction("Details");
        }

        /// <summary>
        /// The clear cart.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IActionResult ClearCart()
        {
            Cart = Cart.ClearCart(HttpContext);
            return RedirectToAction("Details");
        }

        /// <summary>
        /// The update specification.
        /// </summary>
        /// <param name="specificationId">
        /// The specification Id.
        /// </param>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        public IActionResult UpdateSpecification(int specificationId, int quantity)
        {
            var specification = this.db.Specifications
                .Include(sp => sp.Product)
                .ThenInclude(p => p.ProductType)
                .Include(sp => sp.SpecificationValues)
                .ThenInclude(sv => sv.SpecificationTitle)
                .FirstOrDefault(sp => sp.Id == specificationId);

            Cart = Cart.UpdateSpecifications(specification, quantity);
            return RedirectToAction("Details");
        }
    }
}