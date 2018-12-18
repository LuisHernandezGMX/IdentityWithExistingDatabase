using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class IdentityRole : IRole<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}