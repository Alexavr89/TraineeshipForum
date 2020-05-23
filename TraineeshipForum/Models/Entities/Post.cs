using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TraineeshipForum.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual IEnumerable<Reply> Replies { get; set; }
    }
}
