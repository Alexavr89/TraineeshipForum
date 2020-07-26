using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services.User
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        Task SetProfileImage(string id, Uri uri);
        IEnumerable<ApplicationUser> GetAll();
    }
}
