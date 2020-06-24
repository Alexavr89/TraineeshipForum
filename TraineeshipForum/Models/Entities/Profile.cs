using Microsoft.AspNetCore.Http;
using System;

namespace TraineeshipForum.Models.Entities
{
    public class Profile
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
