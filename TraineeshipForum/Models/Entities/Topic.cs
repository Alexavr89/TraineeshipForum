using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
    }
}