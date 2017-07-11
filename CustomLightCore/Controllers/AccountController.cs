using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CustomLightCore.Models;
using CustomLightCore.ViewModels;
using CryptoHelper;


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
				User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && VerifyPassword(u.Password, model.Password));
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
				/////////// Хэширование пароля //////////
				string modelHashedPassword = HashPassword(model.Password);
				/////////////////////////////////////////
				db.Users.Add(new User { Login = model.Login, Password = modelHashedPassword });
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

		// Hash a password
		public string HashPassword(string password)
		{
			return Crypto.HashPassword(password);
		}

		// Verify the password hash against the given password
		public bool VerifyPassword(string hash, string password)
		{
			return Crypto.VerifyHashedPassword(hash, password);
		}
	}
}