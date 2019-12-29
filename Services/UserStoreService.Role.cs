using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    /// <summary>
    /// Implementation of the IRoleStore and IUserRoleStore parts of UserStoreService.
    /// </summary>
    public partial class UserStoreService : IRoleStore<Role>, IUserRoleStore<User>
    {
        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Insert(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Update(role.Id, role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _roles.Delete(role.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        Task<Role> IRoleStore<Role>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleId == null)
                throw new ArgumentNullException(nameof(roleId));

            var role = _roles.FindById(roleId);
            return Task.FromResult(role);
        }

        Task<Role> IRoleStore<Role>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedRoleName == null)
                throw new ArgumentNullException(nameof(normalizedRoleName));

            var role = _roles.FindOne(r => r.NormalizedName == normalizedRoleName);
            return Task.FromResult(role);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            role.Name = roleName ?? throw new ArgumentNullException(nameof(roleName));
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            role.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));
            return Task.FromResult<object>(null);
        }

        public async Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await ((IRoleStore<Role>)this).FindByNameAsync(normalizedRoleName, cancellationToken);
            var userName = user.NormalizedUserName;
            if (!role.UsersInRole.Contains(userName))
            {
                role.UsersInRole.Add(userName);
                _roles.Update(role);
            }
            if (!user.Roles.Contains(role.Name))
            {
                user.Roles.Add(role.Name);
                _users.Update(user);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await ((IRoleStore<Role>)this).FindByNameAsync(normalizedRoleName, cancellationToken);
            var userName = user.NormalizedUserName;
            if (role.UsersInRole.Contains(userName))
            {
                role.UsersInRole.Remove(userName);
                _roles.Update(role);
            }
            if (user.Roles.Contains(role.Name))
            {
                user.Roles.Remove(role.Name);
                _users.Update(user);
            }
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(new List<string>(user.Roles));
        }

        public Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(role));
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var result = new List<User>();
            var role = await ((IRoleStore<Role>)this).FindByNameAsync(roleName, cancellationToken);
            foreach (var user in role.UsersInRole)
                result.Add(await FindByNameAsync(user, cancellationToken));

            return result;
        }
    }
}
