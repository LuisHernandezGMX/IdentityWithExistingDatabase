using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using IdentityWithExistingDatabase.Models.DAL.Entities;

namespace IdentityWithExistingDatabase.Migrations
{
    sealed class Configuration : DbMigrationsConfiguration<IdentityDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(IdentityDBContext context)
        {
            context.Roles.AddOrUpdate(new[] {
                new IdentityRole { Id = 1, Name = "Admin" },
                new IdentityRole { Id = 2, Name = "Editor" },
                new IdentityRole { Id = 3, Name = "Visitor" }
            });

            context.SaveChanges();

            context.Users.AddOrUpdate(user => user.Email, new[] {
                new User {
                    Id = 1,
                    Name = "Súper Admin",
                    Email = "admin@example.com",
                    Birthday = new DateTime(1980, 1, 1),
                    Gender = UserGender.NonSpecified,
                    Password = "AIMeeWpVYA7yeKA6usT0TGaZA/gVcG42AhH6zm/SXQibRncJnafxzv7Qv2xtRa2s4A==", // = Admin1$
                    UserName = "admin@example.com"

                },
                new User {
                    Id = 2,
                    Name = "Súper Editor",
                    Email = "editor@example.com",
                    Birthday = new DateTime(1980, 1, 1),
                    Gender = UserGender.Female,
                    Password = "ANdAE9H+bwtZatUw/U0axcQdL54vvfwjEYyOC/RvmGfEis1PnymmuBpvrIAzLEAIkQ==", // = Editor1$
                    UserName = "editor@example.com"
                },
                new User {
                    Id = 3,
                    Name = "Súper Visitor",
                    Email = "visitor@example.com",
                    Birthday = new DateTime(1980, 1, 1),
                    Gender = UserGender.Male,
                    Password = "AIibEzmSdVnHV92crtSDBlS92hDyJG/yJEYa05ovC3bvJC0wl91tSp8chrYzDYQ8Sg==", // = Visitor1$
                    UserName = "visitor@example.com"
                }
            });

            context.SaveChanges();

            context
                .Users
                .FirstOrDefault(user => user.Id == 1)
                .Roles
                .Add(context.Roles.FirstOrDefault(role => role.Id == 1));

            context
                .Users
                .FirstOrDefault(user => user.Id == 2)
                .Roles
                .Add(context.Roles.FirstOrDefault(role => role.Id == 2));

            context
                .Users
                .FirstOrDefault(user => user.Id == 3)
                .Roles
                .Add(context.Roles.FirstOrDefault(role => role.Id == 3));

            context.SaveChanges();

            context.Tasks.AddOrUpdate(new[] {
                new Task {
                    Id = 1,
                    Description = "My First Task",
                    Users = new[] { context.Users.FirstOrDefault(user => user.Id == 1) }
                },
                new Task {
                    Id = 2,
                    Description = "Task Number 2",
                    Users = new[] { context.Users.FirstOrDefault(user => user.Id == 2) }
                },
                new Task {
                    Id = 3,
                    Description = "Task Number 3",
                    Users = new[] { context.Users.FirstOrDefault(user => user.Id == 3) }
                },
            });

            context.SaveChanges();

            context.Milestones.AddOrUpdate(new[] {
                new Milestone {
                    Id = 1,
                    Description = "The first description",
                    Finished = true,
                    TaskId = 1
                },
                new Milestone {
                    Id = 2,
                    Description = "The second description",
                    Finished = true,
                    TaskId = 2
                },
                new Milestone {
                    Id = 3,
                    Description = "The third description",
                    Finished = false,
                    TaskId = 3
                },
            });
        }
    }
}
