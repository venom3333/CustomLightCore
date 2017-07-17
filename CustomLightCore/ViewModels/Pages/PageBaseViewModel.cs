using CustomLightCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLightCore.ViewModels.Pages
{
	public abstract class PageBaseViewModel
	{
		public static async Task<Page> Get(int? id)
		{
			if (id == null)
			{
				return null;
			}

			Page page = new Page();
			using (CustomLightContext db = new CustomLightContext())
			{
				page = await db.Pages.FindAsync(id);
			}
			return page;
		}
	}
}
