using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class Milestone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool Finished { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
    }
}