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
					ModelState.AddModelError("", "������������ ����� �(���) ������!");
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
				// ��������� ������������ � ��
				/////////// ����������� ������ //////////
				string modelHashedPassword = HashPassword(model.Password);
				/////////////////////////////////////////
				db.Users.Add(new User { Login = model.Login, Password = modelHashedPassword });
				await db.SaveChangesAsync();

				User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
				if (user != null)
				{
					await Authenticate(model.Login); // ��������������

					return RedirectToAction("Index", "Categories");
				}
				else
				{
					ModelState.AddModelError("", "������������ ����� �(���) ������!");
				}
			}
			return View(model);
		}

		private async Task Authenticate(string login)
		{
			// ������� ���� claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, login)
			};

			// ������� ������ ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

			// ��������� ������������������ ����
			// TODO: �������� ����� "Abrakadabra" �� ���-�� ������
			await HttpContext.Authentication.SignInAsync("Abrakadabra", new ClaimsPrincipal(id));
		}

		public async Task<IActionResult> Logout()
		{
			// TODO: �������� ����� "Abrakadabra" �� ���-�� ������
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