using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Tasks = System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IdentityWithExistingDatabase.Models.DAL.Entities
{
    public class RoleStore : IRoleStore<IdentityRole, int>, IQueryableRoleStore<IdentityRole, int>
    {
        private IdentityDBContext db;

        public IQueryable<IdentityRole> Roles {
            get {
                return db.Roles;
            }
        }

        public RoleStore(IdentityDBContext db)
        {
            this.db = db;
        }

        public Tasks.Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var role = db.Roles.FirstOrDefault(rl => rl.Id == roleId);

            return Tasks.Task.FromResult(role);
        }

        public Tasks.Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = db.Roles.FirstOrDefault(rl => rl.Name == roleName);

            return Tasks.Task.FromResult(role);
        }

        public Tasks.Task CreateAsync(IdentityRole role)
        {
            db.Roles.Add(role);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task UpdateAsync(IdentityRole role)
        {
            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        public Tasks.Task DeleteAsync(IdentityRole role)
        {
            db.Roles.Remove(role);
            db.SaveChanges();

            return Tasks.Task.FromResult<object>(null);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    db.Dispose();
                }

                db = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}