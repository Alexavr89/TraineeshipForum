using Microsoft.AspNetCore.Identity;
using System;

namespace TraineeshipForum.Models.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}