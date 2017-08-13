namespace CustomLightCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CustomLightCore.Models;
    using CustomLightCore.ViewModels.Cart;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    /// <summary>
    /// The base controller.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// The db.
        /// </summary>
        protected CustomLightContext db = new CustomLightContext();

        /// <summary>
        /// Gets or sets the cart.
        /// </summary>
        protected CartViewModel Cart { get; set; }

        /// <summary>
        /// The create view bag.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
        protected async Task<bool> CreateViewBag()
        {
            ViewBag.Categories = await db.Categories.ToListAsync();
            ViewBag.Projects = await db.Projects.ToListAsync();
            ViewBag.Pages = await db.Pages.ToListAsync();
            ViewBag.Slides = await db.Slides.ToListAsync();
            ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync();

            return true;
        }
    }
}