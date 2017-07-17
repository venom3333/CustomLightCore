using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomLightCore.Controllers
{
	public class BaseController : Controller
	{
		protected CustomLightContext db = new CustomLightContext();

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		protected async Task<bool> CreateViewBag()
		{
			ViewBag.Categories = await db.Categories.ToListAsync();
			ViewBag.Projects = await db.Projects.ToListAsync();
			ViewBag.Pages = await db.Pages.ToListAsync();
			ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync();
			return true;
		}

		public BaseController()
		{
			//ViewBag.Categories = db.Categories.ToList();
			//ViewBag.Projects = db.Projects.ToList();
			//ViewBag.Pages = db.Pages.ToList();
			//ViewBag.Essentials = db.Essentials.FirstOrDefault();
		}
	}
}