using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public enum UserGender
    {
        Male,
        Female,
        [Display(Name = "Non Specified")]
        NonSpecified
    }

    public class User : IUser<int>
    {
        public User()
        {
            Tasks = new HashSet<Task>();
            Roles = new HashSet<IdentityRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public UserGender Gender { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<IdentityRole> Roles { get; set; }
    }
}