using System.ComponentModel.DataAnnotations;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Actions.WithTopics
{
    public class DeleteTopic : Topic
    {
        public int CategoryId { get; set; }
        [Display(Name = "Topic Title")]
        public string TopicTitle { get; set; }
    }
}
