using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create([Bind("Title, Description, Created")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.Categories.Any(c => c.Title == category.Title))
                    {
                        ModelState.AddModelError("Title", "Category with this title already exists");
                        return View(category);
                    }
                    category.Created = DateTime.Now;

                    _context.Add(category);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again...");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id)
        {
            var categoryToUpdate = _context.Categories.Find(id);

            if (await TryUpdateModelAsync(categoryToUpdate,
                "",
                c => c.Title, c => c.Description))
            {
                if (_context.Categories.Any(c => c.Title == categoryToUpdate.Title))
                {
                    ModelState.AddModelError("Title", "Category with this title already exists");
                    return View(categoryToUpdate);
                }
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(categoryToUpdate);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id, bool? saveChangesError = false)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed";
            }

            Category category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Category categoryToDelete = new Category() { Id = id };

                _context.Entry(categoryToDelete).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
