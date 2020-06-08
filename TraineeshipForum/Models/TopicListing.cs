using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models
{
    public class TopicListing
    {
        public int CategoryId { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }
        [Display(Name = "Topic")]
        public string TopicTitle { get; set; }
        [Display(Name = "Replies")]
        public int PostCount { get; set; }
        [Display(Name = "Activity")]
        public string DateCreated { get; set; }
        [Display(Name = "Author")]
        public string AuthorName { get; set; }
    }
}
