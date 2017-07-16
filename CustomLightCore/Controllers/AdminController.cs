using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CustomLightCore.Controllers
{
	public class AdminController : BaseController
	{
		private readonly UserManager<User> userManager;

		public AdminController(UserManager<User> userManager)
		{
			this.userManager = userManager;
		}

		// ������� �������� �������
		//[Authorize(Roles = "admin")]
		[Authorize]
		public async Task<IActionResult> Main()
		{
			User user = await userManager.GetUserAsync(HttpContext.User);
			var userRoles = await userManager.GetRolesAsync(user);

			ViewBag.Message = $"����� ���������� {user.FullName}!";
			if (userRoles.Contains("NormalUser"))
			{
				ViewBag.RoleMessage = "�� - NormalUser.";
			}

			return View();
		}
	}
}