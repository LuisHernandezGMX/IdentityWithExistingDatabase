using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class IdentityRole : IRole<int>
    {
        public IdentityRole()
        {
            Users = new HashSet<User>();
        }


        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}