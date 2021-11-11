using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;
using TraineeshipForum.Services.Categories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TraineeshipForum.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategory _categoryService;
        public CategoriesApiController(ApplicationDbContext context, ICategory categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }
        // GET: api/<CategoriesApiController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _categoryService.GetAll();
        }

        // GET api/<CategoriesApiController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _categoryService.GetById(id);
        }

        // POST api/<CategoriesApiController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.Categories.Any(c => c.Title == category.Title))
                    {
                        ModelState.AddModelError("Title", "Category with this title already exists");
                    }

                    await _categoryService.Add(category);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes. Try again...");
            }
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        // PUT api/<CategoriesApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            var categoryToUpdate = _context.Categories.Find(id);
            if (_context.Categories.Any(c => c.Title == categoryToUpdate.Title))
            {
                return BadRequest("Category with this title already exists");
            }
            try
            {
                categoryToUpdate.Title = category.Title;
                categoryToUpdate.Description = category.Description;
                await _context.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to save changes.");
            }
            return CreatedAtAction(nameof(Get), new { id = categoryToUpdate.Id }, categoryToUpdate);
        }

        // DELETE api/<CategoriesApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var topicToDelete = _categoryService.GetById(id);
            if (topicToDelete == null)
            {
                return NotFound();
            }
            try
            {
                await _categoryService.Delete(id);
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return BadRequest("Unable to delete. Try again...");
            }
            return NoContent();
        }
    }
}
