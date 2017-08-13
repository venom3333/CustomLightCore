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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CustomLightCore.Models;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    /// <summary>
    /// The cart view model.
    /// </summary>
    public class CartViewModel
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static CartViewModel instance;

        /// <summary>
        /// The sync root.
        /// </summary>
        private static object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        public CartViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected CartViewModel(HttpContext context)
        {
            var key = "Cart";
            if (context.Session.GetString(key) == null)
            {
                this.Specifications = new List<Specification>();
                this.SpecificationQuantities = new Dictionary<int, int>();

                // для тестов
                this.Specifications.Add(new Specification
                {
                    Id = 1,
                    Price = 150
                });

                this.SpecificationQuantities.Add(1, 5);

                // Save            
                var str = JsonConvert.SerializeObject(this);
                context.Session.SetString(key, str);
            }
            else
            {
                // Retrieve
                var str = context.Session.GetString(key);
                var currentCart = JsonConvert.DeserializeObject<CartViewModel>(str);
                this.SpecificationQuantities = currentCart.SpecificationQuantities;
                this.Specifications = currentCart.Specifications;
            }
            this.UpdateTotals();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the specidications.
        /// </summary>
        public List<Specification> Specifications { get; set; }

        /// <summary>
        /// Gets or sets the specifications quantity.
        /// </summary>
        public Dictionary<int, int> SpecificationQuantities { get; set; }

        /// <summary>
        /// Gets or sets the total quantity.
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        public int TotalPrice { get; set; }

        /// <summary>
        /// The get instance.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="CartViewModel"/>.
        /// </returns>
        public static CartViewModel GetInstance(HttpContext context)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CartViewModel(context);
                    }
                }
            }

            return instance;
        }


        /// <summary>
        /// Добавить спецификацию или обновить количество у существующей.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="quantity">
        /// The quantity.
        /// </param>
        /// <returns>
        /// The <see cref="CartViewModel"/>.
        /// </returns>
        public CartViewModel UpdateSpecifications(Specification specification, int quantity)
        {
            if (!this.Specifications.Select(sp => sp.Id).Contains(specification.Id))
            {
                this.Specifications.Add(specification);
            }

            this.SpecificationQuantities[specification.Id] = quantity;
            this.UpdateTotals();
            return this;
        }

        /// <summary>
        /// The remove specification.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <returns>
        /// The <see cref="CartViewModel"/>.
        /// </returns>
        public CartViewModel RemoveSpecification(Specification specification)
        {
            this.Specifications.RemoveAll(sp => sp.Id == specification.Id);
            this.SpecificationQuantities.Remove(specification.Id);
            this.UpdateTotals();
            return this;
        }

        /// <summary>
        /// The update totals.
        /// </summary>
        private void UpdateTotals()
        {
            // Обнуляем Общую цену и общее кол-во
            this.TotalPrice = 0;
            this.TotalQuantity = 0;

            // Набираем общую цену
            foreach (var specification in this.Specifications)
            {
                this.TotalPrice += specification.Price * this.SpecificationQuantities[specification.Id];
                this.TotalQuantity += this.SpecificationQuantities[specification.Id];
            }

        }
    }
}
