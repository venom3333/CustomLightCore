using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CustomLightCore.Models;
using CustomLightCore.ViewModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace CustomLightCore.Controllers
{
	public class AccountController : BaseController
	{

		private string PasswordHash(string password)
		{
			string passwordHash = string.Empty;

			// generate a 128-bit salt using a secure PRNG
			byte[] salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
			return passwordHash;
		}

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
				/////////// ����������� ������ //////////
				string modelPasswordHash = PasswordHash(model.Password);
				/////////////////////////////////////////

				User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == modelPasswordHash);
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
				string modelPasswordHash = PasswordHash(model.Password);
				/////////////////////////////////////////
				db.Users.Add(new User { Login = model.Login, Password = modelPasswordHash });
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
	}
}