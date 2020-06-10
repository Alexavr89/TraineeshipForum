using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models.Actions.WithRoles
{
    public class CreateRole
    {
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
