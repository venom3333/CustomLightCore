using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;

namespace CustomLightCore.Controllers
{
    public class BaseController : Controller
    {
		protected CustomLightContext db = new CustomLightContext();

		public BaseController()
		{
			//ViewBag.Category = db.Category.ToList();
			//ViewBag.Projects = db.Projects.ToList();
			//ViewBag.Pages = db.Pages.ToList();
			//ViewBag.Essentials = db.Essentials.FirstOrDefault();
		}
	}
}