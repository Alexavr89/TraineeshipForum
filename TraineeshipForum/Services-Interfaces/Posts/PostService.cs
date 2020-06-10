using Microsoft.EntityFrameworkCore;
using System.Linq;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Posts
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            var post = _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.Topic)
                    .ThenInclude(t => t.User)
                .FirstOrDefault();

            return post;
        }
    }
}
