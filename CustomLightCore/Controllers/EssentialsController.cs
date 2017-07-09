using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomLightCore.Models;

namespace CustomLightCore.Controllers
{
    public class EssentialsController : BaseController
	{
		[ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, VaryByHeader = "User-agent")]
		public FileContentResult GetLogotype(string logoType)
		{
			Essentials essential = _context.Essentials
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