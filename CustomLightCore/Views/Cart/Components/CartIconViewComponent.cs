// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartIconViewComponent.cs" company="CustomLigth">
//   CustomLigth
// </copyright>
// <summary>
//   The cart icon view component.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomLightCore.Views.Cart.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.ViewModels.Cart;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    /// <summary>
    /// The cart icon view component.
    /// </summary>
    public class CartIconViewComponent : ViewComponent
    {
        /// <summary>
        /// Gets or sets the cart.
        /// </summary>
        protected CartViewModel Cart { get; set; }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <returns>
        /// The <see cref="IViewComponentResult"/>.
        /// </returns>
        public IViewComponentResult Invoke()
        {
            Cart = CartViewModel.GetInstance(HttpContext);
            
            return this.View("~/Views/Shared/_CartIcon.cshtml", Cart);
        }
    }
}
