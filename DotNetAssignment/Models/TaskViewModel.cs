using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetAssignment.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}