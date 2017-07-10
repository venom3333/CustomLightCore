using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;

namespace CustomLightCore.Controllers
{
    public class ProjectsController : BaseController
    {
        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.Include(proj => proj.ProjectImages)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (projects == null)
            {
                return NotFound();
            }

			ViewBag.Categories = await _context.Categories.ToListAsync();
			ViewBag.Projects = await _context.Projects.ToListAsync();
			ViewBag.Pages = await _context.Pages.ToListAsync();
			ViewBag.Essentials = await _context.Essentials.FirstOrDefaultAsync(e => e != null);
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
                _context.Add(projects);
                await _context.SaveChangesAsync();
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

            var projects = await _context.Projects.SingleOrDefaultAsync(m => m.Id == id);
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
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
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

            var projects = await _context.Projects
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
            var projects = await _context.Projects.SingleOrDefaultAsync(m => m.Id == id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public FileContentResult GetProjectIcon(int? Id)
		{
			Project projs = _context.Projects
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

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 3600)]
		public FileContentResult GetProjectImage(int? ImageId)
		{
			ProjectImage image = _context.ProjectImages.FirstOrDefault(i => i.Id == ImageId);

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
