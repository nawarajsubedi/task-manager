using DotNetAssignment.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAssignment.Domain
{
    [Table("TodoTask")]
    public class TodoTask
    {
        public TodoTask() { }

        [Key]
        public int Id { get; set; }

        [Column("Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [Column("Description")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Column("IsCompleted")]
        public bool IsCompleted { get; set; }

        [Column("CreatedBy")]
        public string CreatedBy { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [Column("MarkCompletedBy")]
        public string MarkBy { get; set; }

        [Column("MarkedCompletedOn")]
        public string CompletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser User { get; set; }
    }
}