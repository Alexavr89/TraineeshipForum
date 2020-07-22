using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models.Actions.WithRoles
{
    public class EditRole
    {
        public EditRole()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }
        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
