using Microsoft.AspNetCore.Identity;
using System;

namespace TraineeshipForum.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
        public bool IsAdmin { get; set; } //think about it in Roles
        public bool IsActive { get; set; } // remove this function too
    }
}
