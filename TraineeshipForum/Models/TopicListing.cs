using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TraineeshipForum.Models.Entities;

namespace TraineeshipForum.Models
{
    public class TopicListing
    {
        public int CategoryId { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }
        [Display(Name = "Topic")]
        public string TopicTitle { get; set; }
        [Display(Name = "Replies")]
        public int PostCount { get; set; }
        [Display(Name = "Activity")]
        public string DateCreated { get; set; }
        [Display(Name = "Author")]
        public string AuthorName { get; set; }
        public string LastPostCreated { get; set; }
        public double TimeFromLastPost { get; set; }    //check this out
    }
}
