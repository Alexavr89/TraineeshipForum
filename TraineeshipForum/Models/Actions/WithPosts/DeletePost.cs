using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Actions.WithPosts
{
    public class DeletePost : Post
    {
        public int TopicId { get; set; }
        public int CategoryId { get; set; }
        public string PostContent { get; set; }
    }
}
