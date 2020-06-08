using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [MinLength (20)]
        [Required]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
