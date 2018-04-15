using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using LiteDB;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public partial class UserStoreService : IUserStore<User>,
        IUserPasswordStore<User>, IUserEmailStore<User>,
        IUserPhoneNumberStore<User>, IUserLoginStore<User>
    {
        private ILogger _logger;
        private LiteCollection<User> _users;
        private LiteCollection<IdentityRole> _roles;

        public UserStoreService(ILogger<UserStoreService> logger, DbService dbService)
        {
            _logger = logger;
            _users = dbService.Db.GetCollection<User>();
            _users.EnsureIndex(u => u.NormalizedUserName);
            _users.EnsureIndex(u => u.NormalizedEmail);
            _users.EnsureIndex(u => u.ExternalLogins, "$.ExternalLogins[*]");
            _roles = dbService.Db.GetCollection<IdentityRole>();
            _roles.EnsureIndex(u => u.NormalizedName);
        }

        public void Dispose()
        {
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Insert(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Update(user.Id, user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Delete(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            var user = _users.FindById(userId);
            return Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedUserName == null)
                throw new ArgumentNullException(nameof(normalizedUserName));

            var user = _users.FindOne(u => u.NormalizedUserName == normalizedUserName);
            return Task.FromResult(user);
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedEmail == null)
                throw new ArgumentNullException(nameof(normalizedEmail));

            var user = _users.FindOne(u => u.NormalizedEmail == normalizedEmail);
            return Task.FromResult(user);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            user.UserName = userName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (normalizedName == null)
                throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (passwordHash == null)
                throw new ArgumentNullException(nameof(passwordHash));

            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (normalizedEmail == null)
                throw new ArgumentNullException(nameof(normalizedEmail));

            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            user.PhoneNumber = phoneNumber;
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var loginString = $"{login.LoginProvider}|{login.ProviderKey}";
            if (!user.ExternalLogins.Contains(loginString))
                user.ExternalLogins.Add(loginString);
            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (loginProvider == null)
                throw new ArgumentNullException(nameof(loginProvider));
            if (providerKey == null)
                throw new ArgumentNullException(nameof(providerKey));

            user.ExternalLogins.Remove($"{loginProvider}|{providerKey}");
            return Task.FromResult<object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = user.ExternalLogins.Select(l =>
            {
                var parts = l.Split('|');
                return new UserLoginInfo(parts[0], parts[1], parts[0]);
            }).ToArray();
            return Task.FromResult<IList<UserLoginInfo>>(result);
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (loginProvider == null)
                throw new ArgumentNullException(nameof(loginProvider));
            if (providerKey == null)
                throw new ArgumentNullException(nameof(providerKey));

            var loginString = $"{loginProvider}|{providerKey}";
            var user = _users.FindOne(u => u.ExternalLogins.Contains(loginString));
            return Task.FromResult(user);
        }
    }
}