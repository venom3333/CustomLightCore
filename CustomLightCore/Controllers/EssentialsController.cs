using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;
using CustomLightCore.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.ViewModels.Essentials;

namespace CustomLightCore.Controllers
{
    public class EssentialsController : BaseController
	{
		// GET: Pages/Edit
		[Authorize]
		public async Task<IActionResult> Edit()
		{
			var essentials = await EssentialEditViewModel.GetViewModelByModel();

			if (essentials == null)
			{
				return NotFound();
			}

			return View(essentials);
		}

		// POST: Pages/Edit
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit([Bind("Id,Title,About,Address,Phone,Boss,Email,LogoImageData,LogoImageInvertedData")] EssentialEditViewModel updatedEssential)
		{
			if (ModelState.IsValid)
			{
				Essential essential = updatedEssential.GetModelByViewModel();

				try
				{
					db.Update(essential);
					await db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PageExists(essential.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Main", "Admin");
			}
			return View(updatedEssential);
		}

		private bool PageExists(int id)
		{
			throw new NotImplementedException();
		}

		[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "User-agent")]
		public FileContentResult GetLogotype(string logoType)
		{
			Essential essential = db.Essentials
				.FirstOrDefault(c => c != null);

			if (essential != null)
			{
				switch (logoType)
				{
					case "normal":
						if (essential.LogoImageData != null)
						{
							return File(essential.LogoImageData, essential.LogoImageMimeType);
						}
						else
						{
							return null;
						}
					case "inverted":
						if (essential.LogoImageInvertedData != null)
						{
							return File(essential.LogoImageInvertedData, essential.LogoImageInvertedMimeType);
						}
						else
						{
							return null;
						}
					default:
						return null;
				}
			}
			else
			{
				return null;
			}
		}
	}
}