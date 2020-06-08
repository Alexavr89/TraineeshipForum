using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Actions.WithTopics
{
    public class DeleteTopic : Topic
    {
        public int CategoryId { get; set; }
        public string TopicTitle { get; set; }
    }
}
