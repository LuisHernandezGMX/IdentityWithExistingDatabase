using System.Web;
using System.Web.Mvc;
using Tasks = System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IdentityWithExistingDatabase.Models.DAL.Entities;
using IdentityWithExistingDatabase.Models.ViewModels.Account;

namespace IdentityWithExistingDatabase.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            
            HttpContext
                .GetOwinContext()
                .Authentication
                .SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var newUser = new User {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Birthday = model.Birthday.Value,
                    Gender = model.Gender
                };

                using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>())
                using (var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>()) {
                    var result = await userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded) {
                        await userManager.AddToRoleAsync(newUser.Id, "Visitor");
                        await signInManager.SignInAsync(newUser, false, false);

                        return RedirectToAction("Index", "Main");
                    } else {
                        foreach (var error in result.Errors) {
                            ModelState.AddModelError("General", error);
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Tasks.Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) {
                using (var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>()) {
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    switch (result) {
                        case SignInStatus.Success:
                            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                                return Redirect(returnUrl);
                            }

                            return RedirectToAction("Index", "Main");
                        default:
                            ModelState.AddModelError("General", "Invalid log in attempt.");
                            break;
                    }
                }
            }

            return View(model);
        }
    }
}