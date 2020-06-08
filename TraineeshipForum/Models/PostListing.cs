namespace TraineeshipForum.Models
{
    public class PostListing
    {
        public int PostId { get; set; }
        public int TopicId { get; set; }
        public int CategoryId { get; set; }
        public string AuthorName { get; set; }
        public string DatePosted { get; set; }
        public string Content { get; set; }
        public string AuthorImageURL { get; set; }
    }
}
