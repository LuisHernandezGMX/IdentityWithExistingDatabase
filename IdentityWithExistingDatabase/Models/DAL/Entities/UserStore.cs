using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class UserStore
        : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserEmailStore<User, int>,
        IUserRoleStore<User, int>,
        IQueryableUserStore<User, int>
    {
        private IdentityDBContext db;

        #region IQueryableUserStore
        public IQueryable<User> Users {
            get {
                return db.Users;
            }
        }
        #endregion IQueryableUserStore

        public UserStore(IdentityDBContext db)
        {
            this.db = db;
        }

        #region IUserStore
        public Tasks.Task<User> FindByIdAsync(int userId)
        {
            var user = db.Users
                .Include(us => us.Roles)
                .Include(us => us.Tasks)
                .FirstOrDefault(us => us.Id == userId);

            return Tasks.Task.FromResult(user);
        }

        public Tasks.Task<User> FindByNameAsync(string userName)
        {
            var user = db.Users
                .Include(us => us.Roles)
                .Include(us => us.Tasks)
                .FirstOrDefault(us => us.UserName == userName);

            return Tasks.Task.FromResult(user);
        }

        public Tasks.Task CreateAsync(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task UpdateAsync(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task DeleteAsync(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }
        #endregion

        #region IUserPasswordStore
        public Tasks.Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task<string> GetPasswordHashAsync(User user)
        {
            return Tasks.Task.FromResult(user.Password);
        }

        public Tasks.Task<bool> HasPasswordAsync(User user)
        {
            return Tasks.Task.FromResult(!string.IsNullOrWhiteSpace(user.Password));
        }
        #endregion

        #region IUserEmailStore
        public Tasks.Task SetEmailAsync(User user, string email)
        {
            user.Email = email;
            user.UserName = email;

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task<string> GetEmailAsync(User user)
        {
            return Tasks.Task.FromResult(user.Email);
        }

        public Tasks.Task<bool> GetEmailConfirmedAsync(User user)
        {
            // Implementación por defecto (no se confirma si el correo del usuario es válido)
            return Tasks.Task.FromResult(true);
        }

        public Tasks.Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            // Implementación por defecto (no se confirma si el correo del usuario es válido)
            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task<User> FindByEmailAsync(string email)
        {
            var user = db.Users.FirstOrDefault(us => us.Email == email);

            return Tasks.Task.FromResult(user);
        }
        #endregion

        #region IUserRoleStore
        public Tasks.Task<IList<string>> GetRolesAsync(User user)
        {
            var roles = user
                .Roles
                .Select(role => role.Name)
                .ToList();

            return Tasks.Task.FromResult(roles as IList<string>);
        }

        public Tasks.Task<bool> IsInRoleAsync(User user, string roleName)
        {
            bool isInRole = user.Roles.Any(role => role.Name == roleName);

            return Tasks.Task.FromResult(isInRole);
        }

        public Tasks.Task AddToRoleAsync(User user, string roleName)
        {
            var role = db.Roles.FirstOrDefault(rl => rl.Name == roleName);
            user.Roles.Add(role);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task RemoveFromRoleAsync(User user, string roleName)
        {
            var role = db.Roles.FirstOrDefault(rl => rl.Name == roleName);
            user.Roles.Remove(role);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }
        #endregion

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) {
                if (disposing) {
                    db.Dispose();
                }

                db = null;
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose
    }
}