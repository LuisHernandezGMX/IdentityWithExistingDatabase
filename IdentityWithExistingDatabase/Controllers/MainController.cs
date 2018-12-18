using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tasks = System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace IdentityWithExistingDatabase.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public async Tasks.Task<ViewResult> Index()
        {
            using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>()) {
                var user = await userManager.FindByNameAsync(User.Identity.Name);

                return View(user);
            }
        }
    }
}