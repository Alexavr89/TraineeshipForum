using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Categories
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
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
