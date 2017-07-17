using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomLightCore.Controllers
{
    public class ProjectsController : BaseController
    {
        // GET: Projects
		[Authorize]
        public async Task<IActionResult> List()
        {
            return View(await db.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await db.Projects.Include(proj => proj.ProjectImages)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

			ViewBag.Categories = await db.Categories.ToListAsync();
			ViewBag.Projects = await db.Projects.ToListAsync();
			ViewBag.Pages = await db.Pages.ToListAsync();
			ViewBag.Essentials = await db.Essentials.FirstOrDefaultAsync();
			return View(projects);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated")] Project projects)
        {
            if (ModelState.IsValid)
            {
                db.Add(projects);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await db.Projects.SingleOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ShortDescription,Icon,IconMimeType,IsPublished,Created,Updated")] Project projects)
        {
            if (id != projects.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(projects);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await db.Projects
                .SingleOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await db.Projects.SingleOrDefaultAsync(m => m.Id == id);
            db.Projects.Remove(projects);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectsExists(int id)
        {
            return db.Projects.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetProjectIcon(int? Id)
		{
			Project projs = db.Projects
				.FirstOrDefault(p => p.Id == Id);

			if (projs.Icon != null)
			{
				return File(projs.Icon, projs.IconMimeType);
			}
			else
			{
				return null;
			}
		}

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetProjectImage(int? ImageId)
		{
			ProjectImage image = db.ProjectImages.FirstOrDefault(i => i.Id == ImageId);

			if (image != null)
			{
				return File(image.ImageData, image.ImageMimeType);
			}
			else
			{
				return null;
			}
		}
	}
}
