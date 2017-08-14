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

            var specification = this.db.Specifications.FirstOrDefault(sp => sp.Id == specificationId);

            if (specification != null)
            {
                Cart = Cart.AddToCart(specification, quantity);
            }
            return PartialView("~/Views/Shared/_CartIcon.cshtml", Cart);
        }



    }
}