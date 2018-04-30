using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace LanfeustBridge.Services
{
    /// <summary>
    /// Implementation of the IRoleStore part of UserStoreService
    /// </summary>
    public partial class UserStoreService : IRoleStore<IdentityRole>
    {
        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Insert(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Update(role.Id, role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Delete(role.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        Task<IdentityRole> IRoleStore<IdentityRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleId == null)
                throw new ArgumentNullException(nameof(roleId));

            var role = _roles.FindById(roleId);
            return Task.FromResult(role);
        }

        Task<IdentityRole> IRoleStore<IdentityRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedRoleName == null)
                throw new ArgumentNullException(nameof(normalizedRoleName));

            var role = _roles.FindOne(r => r.NormalizedName == normalizedRoleName);
            return Task.FromResult(role);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (roleName == null)
                throw new ArgumentNullException(nameof(roleName));

            role.Name = roleName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (normalizedName == null)
                throw new ArgumentNullException(nameof(normalizedName));

            role.NormalizedName = normalizedName;
            return Task.FromResult<object>(null);
        }
    }
}
