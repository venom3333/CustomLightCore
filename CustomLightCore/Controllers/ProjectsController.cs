using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using CustomLightCore.ViewModels.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomLightCore.Controllers
{
    public class ProjectsController : BaseController
    {
        // GET: Projects
		[Authorize]
        public async Task<IActionResult> List()
        {
			var projects = await db.Projects
				.Include(p => p.CategoryProject)
				.ThenInclude(cp => cp.Categories)
				.ToListAsync();
			return View(projects);
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

            await CreateViewBag();
			return View(projects);
        }

        // GET: Projects/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
                "Id,Name,Description,ShortDescription,Icon,IsPublished,CategoryProjectId,ProjectImages")]
            ProjectViewModel createdProject)
        {
            if (ModelState.IsValid)
            {
                var project = createdProject.GetModelByViewModel();
                
                db.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            
            ViewData["Categories"] = new SelectList(db.Categories, "Id", "Name");
            return View(createdProject);
        }

        // GET: Projects/Edit/5
        [Authorize]
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
        [Authorize]
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
                return RedirectToAction("List");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await db.Projects.SingleOrDefaultAsync(m => m.Id == id);
            db.Projects.Remove(projects);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        private bool ProjectsExists(int id)
        {
            return db.Projects.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetProjectIcon(int? id)
		{
			Project projs = db.Projects
				.FirstOrDefault(p => p.Id == id);

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
		public FileContentResult GetProjectImage(int? imageId)
		{
			ProjectImage image = db.ProjectImages.FirstOrDefault(i => i.Id == imageId);

			if (image != null)
			{
				return File(image.ImageData, image.ImageMimeType);
			}
			else
			{
				return null;
			}
		}
        
        // Вижу / Не вижу
        [HttpPost]
        [Authorize]
        public async Task<bool> TogglePublish([Bind("id")]int? id)
        {
            if (id == null)
            {
                return false;
            }

            var project = await db.Projects
                .SingleOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return false;
            }

            project.IsPublished = !project.IsPublished;
            db.Update(project);
            await db.SaveChangesAsync();

            return true;
        }
	}
}
