using System.Collections.Generic;
using System.Threading.Tasks;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services.Topics
{
    public interface ITopic
    {
        Topic GetById(int id);
        Task Add(Topic topic);
        Task Delete(int id);
        IEnumerable<Topic> GetAllTopics();
        IEnumerable<Topic> GetLatestTopics(int amount);
    }
}
