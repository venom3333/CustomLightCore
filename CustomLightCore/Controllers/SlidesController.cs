using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomLightCore.Models;
using CustomLightCore.ViewModels.Slides;

namespace CustomLightCore.Controllers
{
    public class SlidesController : BaseController
    {
        // GET: Slides
        public async Task<IActionResult> List()
        {
            return View(await db.Slides.ToListAsync());
        }

        // GET: Slides/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Slides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Description")]  SlideCreateViewModel createdSlide)
        {
            if (ModelState.IsValid)
            {
				Slide slide = createdSlide.GetModelByViewModel();

                db.Add(slide);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(createdSlide);
        }

        // GET: Slides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await SlideEditViewModel.GetViewModelByModelId(id);
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        // POST: Slides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Description")] SlideEditViewModel newSlide)
        {
            if (id != newSlide.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				// Старые данные объекта
				Slide slide = newSlide.GetModelByViewModel();

				try
				{


                    db.Update(slide);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.Id))
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
            return View(newSlide);
        }

        // GET: Slides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await db.Slides
                .SingleOrDefaultAsync(m => m.Id == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slide = await db.Slides.SingleOrDefaultAsync(m => m.Id == id);
            db.Slides.Remove(slide);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        private bool SlideExists(int id)
        {
            return db.Slides.Any(e => e.Id == id);
        }

		[ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 60)]
		public FileContentResult GetSlideImage(int? Id)
		{
			Slide slide = db.Slides
				.FirstOrDefault(c => c.Id == Id);

			if (slide.ImageData != null)
			{
				return File(slide.ImageData, slide.ImageMimeType);
			}
			else
			{
				return null;
			}
		}
	}
}
