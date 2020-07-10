using TraineeshipForum.Models.Actions.WithPosts;
using TraineeshipForum.Models.Actions.WithTopics;

namespace TraineeshipForum.Models.Actions
{
    public class NewTopicandPost
    {
        public NewTopic Topic { get; set; }
        public NewPost Post { get; set; }
        public int CategoryId { get; set; }
    }
}
