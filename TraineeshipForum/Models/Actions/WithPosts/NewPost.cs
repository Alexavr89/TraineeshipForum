using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Actions.WithPosts
{
    public class NewPost : Post
    {
        public int CategoryId { get; set; }
        public int TopicId { get; set; }
    }
}
