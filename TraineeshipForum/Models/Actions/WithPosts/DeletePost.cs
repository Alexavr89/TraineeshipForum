using System.ComponentModel.DataAnnotations;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Actions.WithPosts
{
    public class DeletePost : Post
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Post Content")]
        public string PostContent { get; set; }
    }
}
