using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IdentityWithExistingDatabase.Models.DAL.Entities;

namespace IdentityWithExistingDatabase.Models.ViewModels.User
{
    public enum Role
    {
        Admin,
        Editor,
        Visitor
    }

    public class IndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }

        [Required]
        public UserGender Gender { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class EditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }

        [Required]
        public UserGender Gender { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Password { get; set; }
    }
}