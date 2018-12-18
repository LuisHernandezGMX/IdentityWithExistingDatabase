using System.Data.Entity.Utilities;
using Tasks = System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IdentityWithExistingDatabase.Models.DAL.Entities;

namespace IdentityWithExistingDatabase
{
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore(context.Get<IdentityDBContext>()));

            manager.UserLockoutEnabledByDefault = false;

            manager.UserValidator = new UserValidator<User, int>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator {
                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            if (options.DataProtectionProvider != null) {
                var dataProtector = options.DataProtectionProvider.Create("ASP.NET Identity");
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtector);
            }

            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(UserManager<User, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override async Tasks.Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (UserManager == null) {
                return SignInStatus.Failure;
            }

            var user = await UserManager.FindByNameAsync(userName).WithCurrentCulture();

            if (user == null) {
                return SignInStatus.Failure;
            }

            if (await UserManager.CheckPasswordAsync(user, password).WithCurrentCulture()) {
                await SignInAsync(user, isPersistent, false).WithCurrentCulture();
                return SignInStatus.Success;
            }

            return SignInStatus.Failure;
        }
    }
}