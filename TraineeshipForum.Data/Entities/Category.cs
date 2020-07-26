using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraineeshipForum.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual IEnumerable<Topic> Topics { get; set; }
    }
}