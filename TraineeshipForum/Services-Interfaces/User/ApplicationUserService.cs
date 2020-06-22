using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraineeshipForum.Data;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Services_Interfaces.User
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        public ApplicationUser GetById(string id)
        {
            return _context.ApplicationUsers.FirstOrDefault(user => user.Id == id);
        }
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }
        public async Task Deactivate(ApplicationUser user)
        {
            user.IsActive = false;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
