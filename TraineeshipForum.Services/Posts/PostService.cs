using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services.Posts
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post post)
        {
            post.Created = DateTime.Now;
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var postToDelete = GetById(id);
            _context.Entry(postToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
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
