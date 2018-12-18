using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class IdentityDBContext : DbContext
    {
        public IdentityDBContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
    }
}