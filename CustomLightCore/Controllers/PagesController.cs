using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;
using CustomLightCore.ViewModels.Pages;

namespace CustomLightCore.Controllers
{
    public class PagesController : BaseController
    {
		// GET: Pages
		[Authorize]
		public async Task<IActionResult> List()
        {
            return View(await db.Pages.ToListAsync());
        }	

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await db.Pages
                .SingleOrDefaultAsync(p => p.Id == id);
            if (page == null)
            {
                return NotFound();
            }

			await CreateViewBag();
			return View(page);
        }

        // GET: Pages/Create
		[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("Id,Alias,Name,PageContent")] Page page)
        {
            if (ModelState.IsValid)
            {
				var now = DateTime.Now;
				page.Created = now;
				page.Updated = now;

				db.Add(page);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(page);
        }

		// GET: Pages/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await db.Pages.SingleOrDefaultAsync(m => m.Id == id); // TODO: (PageEditViewModel.Get(id))
			
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Alias,Name,PageContent")] Page newPageData)
        {
            if (id != newPageData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				// Старые данные объекта
				Page oldPageData = await db.Pages.FirstOrDefaultAsync(p => p.Id == id);



                try
                {
                    db.Update(newPageData);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(newPageData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("List");
            }
            return View(newPageData);
        }

		// GET: Pages/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await db.Pages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await db.Pages.SingleOrDefaultAsync(m => m.Id == id);
            db.Pages.Remove(page);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        private bool PageExists(int id)
        {
            return db.Pages.Any(e => e.Id == id);
        }
    }
}
