using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TraineeshipForum.Models.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }
    }
}