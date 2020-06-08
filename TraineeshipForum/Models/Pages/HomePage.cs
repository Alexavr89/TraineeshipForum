using System.Collections.Generic;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models.Pages
{
    public class HomePage
    {
        public IEnumerable<TopicListing> Topics { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
