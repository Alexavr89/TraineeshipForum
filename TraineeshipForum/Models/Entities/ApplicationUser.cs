using Microsoft.AspNetCore.Identity;

namespace TraineeshipForum.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
    }
}
