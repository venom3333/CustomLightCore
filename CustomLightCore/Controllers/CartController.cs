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
            // Для тестов
            var cart = CartViewModel.GetInstance(HttpContext);
            cart.UpdateSpecifications(
                new Specification
                {
                    Id = 2,
                    Price = 120,
                },
            7);

            await CreateViewBag();
            return View(cart);
        }

    }
}