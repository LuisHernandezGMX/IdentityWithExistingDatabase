using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class Task
    {
        public Task()
        {
            Users = new HashSet<User>();
            Milestones = new HashSet<Milestone>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Milestone> Milestones { get; set; }
    }
}