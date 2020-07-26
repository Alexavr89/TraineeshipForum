using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services.Topics
{
    public class TopicService : ITopic
    {
        private readonly ApplicationDbContext _context;

        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Topic topic)
        {
            topic.Created = DateTime.Now;
            _context.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var topicToDelete = GetById(id);
            _context.Entry(topicToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Topic> GetAllTopics()
        {
            var topics = _context.Topics
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.User);

            return topics;
        }

        public Topic GetById(int id)
        {
            var topic = _context.Topics
                .Where(t => t.Id == id)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.User)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.Topic)
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefault();

            if (topic.Posts == null)
            {
                topic.Posts = new List<Post>();
            }

            return topic;
        }

        public IEnumerable<Topic> GetLatestTopics(int amount)
        {
            var allTopics = GetAllTopics().OrderByDescending(topic => topic.Created);
            return allTopics.Take(amount);
        }
    }
}
