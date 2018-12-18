using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using IdentityWithExistingDatabase.Models.DAL.Entities;
using IdentityWithExistingDatabase.Models.ViewModels.Task;

namespace IdentityWithExistingDatabase.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        public ViewResult AssignedTasks()
        {
            using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                var model = new AssignedTasksViewModel();
                var tasks = db.Tasks
                    .Include(tsk => tsk.Users)
                    .ToList();

                foreach (var task in tasks) {
                    var taskModel = new AssignedTaskViewModel {
                        Id = task.Id,
                        Description = task.Description
                    };

                    var usersModel = task.Users
                        .Select(usr => new AssignedUserViewModel {
                            Id = usr.Id,
                            Name = usr.Name,
                            Email = usr.Email
                        })
                        .ToList();

                    model.UsersByTask[taskModel] = usersModel;
                }

                return View(model);
            }
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ViewResult Index()
        {
            using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                var tasks = db.Tasks.ToList();

                return View(tasks);
            }
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ViewResult Details(int id)
        {
            using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                var task = db.Tasks
                    .Include(tsk => tsk.Users)
                    .Include(tsk => tsk.Milestones)
                    .FirstOrDefault(tsk => tsk.Id == id);

                return View(task);
            }
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ViewResult CreateMilestone(int taskId)
        {
            ViewBag.TaskId = taskId;
            return View();
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ViewResult AssignUsers(int taskId)
        {
            using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                var users = db.Users
                    .Where(user => !user.Tasks.Any(task => task.Id == taskId))
                    .Select(user => new AssignedUserViewModel {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    })
                    .ToList();

                var model = new AssignUsersViewModel {
                    TaskId = taskId,
                    AssignedUsers = users
                };

                ViewBag.AssignUsers = model;
                return View();
            }   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid) {
                var task = new Task { Description = model.Description };

                using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                    db.Tasks.Add(task);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ActionResult CreateMilestone(CreateMilestoneViewModel model, int taskId)
        {
            if (ModelState.IsValid) {
                var milestone = new Milestone {
                    Description = model.Description,
                    Finished = model.Finished
                };

                using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                    var task = db.Tasks.FirstOrDefault(tsk => tsk.Id == taskId);

                    task.Milestones.Add(milestone);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = taskId });
                }
            }

            ViewBag.TaskId = taskId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin,Editor")]
        public ActionResult AssignUsers(BindAssignedUsersViewModel model)
        {
            using (var db = HttpContext.GetOwinContext().Get<IdentityDBContext>()) {
                var task = db.Tasks.FirstOrDefault(tsk => tsk.Id == model.TaskId);
                
                foreach (var userId in model.Users) {
                    task.Users.Add(db.Users.FirstOrDefault(user => user.Id == userId));
                }

                db.SaveChanges();
                return RedirectToAction("Details", new { id = model.TaskId });
            }
        }
    }
}