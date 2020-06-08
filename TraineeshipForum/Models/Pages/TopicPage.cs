using System.Collections.Generic;

namespace TraineeshipForum.Models.Pages
{
    public class TopicPage
    {
        public int CategoryId { get; set; }
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public string CategoryTitle { get; set; }
        public IEnumerable<PostListing> Posts { get; set; }
    }
}
