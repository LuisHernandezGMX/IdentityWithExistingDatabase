using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityWithExistingDatabase
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                    controller = "Home",
                    action = "Index"
                }));
            } else {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
