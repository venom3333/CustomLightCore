// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartViewModel.cs" company="Custom-light">
//   Custom-Light
// </copyright>
// <summary>
//   Defines the CartViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace CustomLightCore.ViewModels.Cart
{
    using System.Collections.Generic;

    using CustomLightCore.Models;

    /// <summary>
    /// The cart view model.
    /// </summary>
    public class CartViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the specidications.
        /// </summary>
        public List<Specification> Specidications { get; set; }

        /// <summary>
        /// Gets or sets the specifications quantity.
        /// </summary>
        public List<int> SpecificationsQuantity { get; set; }

        /// <summary>
        /// Gets or sets the total quantity.
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public int TotalPrice { get; set; }
    }
}
