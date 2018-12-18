using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using IdentityWithExistingDatabase.Models.DAL.Entities;

namespace IdentityWithExistingDatabase.Models.ViewModels.Account
{
    public class RegisterViewModel
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
        [StringLength(35, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Password { get; set; }
    }
}