using System;
using System.Collections.Generic;

namespace TraineeshipForum.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual IEnumerable<Topic> Topics { get; set; }
    }
}