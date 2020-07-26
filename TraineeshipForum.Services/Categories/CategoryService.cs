using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services.Categories
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Category category)
        {
            category.Created = DateTime.Now;

            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Category categoryToDelete = new Category() { Id = id };
            _context.Entry(categoryToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories
                .Include(c => c.Topics)
                    .ThenInclude(t => t.User)
                .Include(c => c.Topics)
                    .ThenInclude(t => t.Posts)
                        .ThenInclude(p => p.User);

            return categories;
        }

        public Category GetById(int id)
        {
            var category = _context.Categories
                .Where(c => c.Id == id)
                .Include(c => c.Topics)
                    .ThenInclude(t => t.User)
                .Include(c => c.Topics)
                    .ThenInclude(t => t.Posts)
                        .ThenInclude(p => p.User)
                .Include(c => c.Topics)
                    .ThenInclude(t => t.Category)
                .FirstOrDefault();

            if (category.Topics == null)
            {
                category.Topics = new List<Topic>();
            }

            return category;
        }
    }
}
