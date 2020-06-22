using System.Collections.Generic;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models
{
    public class ProfileListing
    {
        public IEnumerable<Profile> Profiles { get; internal set; }
    }
}
