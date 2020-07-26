using Microsoft.AspNetCore.Identity;
using System;

namespace TraineeshipForum.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
    }
}
