using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tasks = System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using IdentityWithExistingDatabase.Models.DAL.Entities;
using IdentityWithExistingDatabase.Models.ViewModels.User;

namespace IdentityWithExistingDatabase.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class UserController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>()) {
                var users = userManager
                    .Users
                    .Where(user => user.UserName != User.Identity.Name)
                    .Select(user => new IndexViewModel {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Roles = user.Roles.Select(role => role.Name).ToList()
                    })
                    .ToList();

                return View(users);
            }
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Tasks.Task<ViewResult> Details(int id)
        {
            using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>()) {
                var user = await userManager.FindByIdAsync(id);

                return (user == null)
                    ? View("Index")
                    : View(user);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid) {
                var newUser = new User {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Birthday = model.Birthday.Value,
                    Gender = model.Gender
                };

                using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>()) {
                    var result = await userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded) {
                        await userManager.AddToRoleAsync(newUser.Id, model.Role.ToString());

                        return RedirectToAction("Index");
                    } else {
                        foreach (var error in result.Errors) {
                            ModelState.AddModelError("General", error);
                        }
                    }
                }
            }

            return View(model);
        }
    }
}