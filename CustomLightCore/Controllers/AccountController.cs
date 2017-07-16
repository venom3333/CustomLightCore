using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CustomLightCore.Models;
using CustomLightCore.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CustomLightCore.Controllers
{
	public class AccountController : BaseController
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> loginManager;
		private readonly RoleManager<Role> roleManager;

		public AccountController(UserManager<User> userManager, SignInManager<User> loginManager, RoleManager<Role> roleManager)
		{
			this.userManager = userManager;
			this.loginManager = loginManager;
			this.roleManager = roleManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var result = loginManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false).Result;

				if (result.Succeeded)
				{
					return RedirectToAction("Main", "Admin");
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
		public IActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new User()
				{
					UserName = model.UserName,
					Email = model.Email,
					FullName = model.FullName,
					BirthDate = model.BirthDate
				};

				IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

				if (result.Succeeded)
				{
					if (!roleManager.RoleExistsAsync("NormalUser").Result)
					{
						Role role = new Role
						{
							Name = "NormalUser",
							Description = "Разрешены обычные операции"
						};

						IdentityResult roleResult = roleManager.CreateAsync(role).Result;

						if (!roleResult.Succeeded)
						{
							ModelState.AddModelError("", "Ошибка при создании роли!");
							return View(model);
						}
					}
					IdentityResult result1212 = userManager.AddToRoleAsync(user, "NormalUser").Result;
					return RedirectToAction("Login", "Account");
				}
			}
			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await loginManager.SignOutAsync();
			return RedirectToAction("Index", "Categories");
		}

	}
}