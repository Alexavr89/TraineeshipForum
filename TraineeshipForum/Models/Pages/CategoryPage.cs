using System.Collections.Generic;

namespace TraineeshipForum.Models.Pages
{
    public class CategoryPage
    {
        public CategoryListing Category { get; set; }
        public IEnumerable<TopicListing> Topics { get; set; }
        public IEnumerable<CategoryListing> Categories { get; set; }
    }
}
