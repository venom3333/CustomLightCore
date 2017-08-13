namespace CustomLightCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            return View();
        }

        /// <summary>
        /// The render cart icon.
        /// </summary>
        /// <returns>
        /// The <see cref="PartialViewResult"/>.
        /// </returns>
        public PartialViewResult RenderCartIcon()
        {
            var model = Cart;
            return PartialView("~/Views/Shared/_CartIcon.cshtml", model);
        }
    }
}