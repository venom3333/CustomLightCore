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
		protected CustomLightContext _context = new CustomLightContext();

		public BaseController()
		{
			//ViewBag.Categories = _context.Categories.ToList();
			//ViewBag.Projects = _context.Projects.ToList();
			//ViewBag.Pages = _context.Pages.ToList();
			//ViewBag.Essentials = _context.Essentials.FirstOrDefault(e => e != null);
		}
	}
}