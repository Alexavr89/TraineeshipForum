using System.Collections.Generic;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.Topics
{
    public interface ITopic
    {
        Topic GetById(int id);
        IEnumerable<Topic> GetAllTopics();
        IEnumerable<Topic> GetLatestTopics(int amount);
    }
}
