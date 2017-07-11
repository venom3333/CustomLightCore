using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CustomLightCore.Models;
using CustomLightCore.ViewModels;

namespace CustomLightCore.Controllers
{
	public class AccountController : BaseController
	{
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
				if (user != null)
				{
					await Authenticate(model.Login);

					return RedirectToAction("Index", "Categories");
				}
				else
				{
					ModelState.AddModelError("", "Некорректные логин и(или) пароль!");
				}
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// добавляем пользователя в бд
				db.Users.Add(new User { Login = model.Login, Password = model.Password });
				await db.SaveChangesAsync();

				User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
				if (user != null)
				{
					await Authenticate(model.Login); // аутентификация

					return RedirectToAction("Index", "Categories");
				}
				else
				{
					ModelState.AddModelError("", "Некорректные логин и(или) пароль!");
				}
			}
			return View(model);
		}

		private async Task Authenticate(string login)
		{
			// создаем один claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login)
			};

			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

			// установка аутентификационных куки
			// TODO: Поменять слово "Abrakadabra" на что-то другое
			await HttpContext.Authentication.SignInAsync("Abrakadabra", new ClaimsPrincipal(id));
		}

		public async Task<IActionResult> Logout()
		{
			// TODO: Поменять слово "Abrakadabra" на что-то другое
			await HttpContext.Authentication.SignOutAsync("Abrakadabra");
			return RedirectToAction("Login", "Account");
		}
	}
}