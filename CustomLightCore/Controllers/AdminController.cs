using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CustomLightCore.Controllers
{
	public class AdminController : BaseController
	{
		// Главная страница админки
		//[Authorize(Roles = "admin")]
		[Authorize]
		public IActionResult Main()
		{
			return View();
		}

		//// категории
		//public async Task<IActionResult> CategoriesList()
		//{
		//	return View(await db.Categories.ToListAsync());
		//}

		//// продукты
		//public async Task<IActionResult> ProductsList()
		//{
		//	return View(await db.Products.ToListAsync());
		//}

		//// проекты
		//public async Task<IActionResult> ProjectsList()
		//{
		//	return View(await db.Projects.ToListAsync());
		//}

		//// заказы
		//public async Task<IActionResult> OrdersList()
		//{
		//	return View(await db.Orders.ToListAsync());
		//}

		//// Основная информация
		//public async Task<IActionResult> Essentials()
		//{
		//	return View(await db.Essentials.FirstOrDefaultAsync());
		//}

		//// Слайды главной страницы
		//public async Task<IActionResult> SlidesList()
		//{
		//	return View(await db.Slides.ToListAsync());
		//}

		//public async Task<IActionResult> PagesList()
		//{
		//	return View(await db.Pages.ToListAsync());
		//}

	}
}