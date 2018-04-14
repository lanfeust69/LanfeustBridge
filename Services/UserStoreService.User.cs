using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using LiteDB;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Services
{
    public partial class UserStoreService : IUserStore<IdentityUser>, 
        IUserPasswordStore<IdentityUser>, IUserEmailStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>, IUserLoginStore<IdentityUser>
    {
        private ILogger _logger;
        private LiteCollection<IdentityUser> _users;
        private LiteCollection<IdentityRole> _roles;
        private Dictionary<string, string> _usersByGoogleId = new Dictionary<string, string>();
        private Dictionary<string, List<UserLoginInfo>> _usersLogins = new Dictionary<string, List<UserLoginInfo>>();

        public UserStoreService(ILogger<UserStoreService> logger, DbService dbService)
        {
            _logger = logger;
            _users = dbService.Db.GetCollection<IdentityUser>();
            _users.EnsureIndex(u => u.NormalizedUserName);
            _users.EnsureIndex(u => u.NormalizedEmail);
            _roles = dbService.Db.GetCollection<IdentityRole>();
            _roles.EnsureIndex(u => u.NormalizedName);
        }

        public void Dispose()
        {
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Insert(user);
            _usersLogins[user.Id] = new List<UserLoginInfo>();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Update(user.Id, user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Delete(user.Id);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            var user = _users.FindById(userId);
            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedUserName == null)
                throw new ArgumentNullException(nameof(normalizedUserName));

            var user = _users.FindOne(u => u.NormalizedUserName == normalizedUserName);
            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedEmail == null)
                throw new ArgumentNullException(nameof(normalizedEmail));

            var user = _users.FindOne(u => u.NormalizedEmail == normalizedEmail);
            return Task.FromResult(user);
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            user.UserName = userName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (normalizedName == null)
                throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (passwordHash == null)
                throw new ArgumentNullException(nameof(passwordHash));

            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            user.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (normalizedEmail == null)
                throw new ArgumentNullException(nameof(normalizedEmail));

            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            user.PhoneNumber = phoneNumber;
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            _usersByGoogleId[login.ProviderKey] = user.Id;
            _usersLogins[user.Id].Add(login);
            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            _usersByGoogleId.Remove(providerKey);
            _usersLogins[user.Id].RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
            return Task.FromResult<object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<UserLoginInfo>>(_usersLogins[user.Id]);
        }

        public Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            _usersByGoogleId.TryGetValue(providerKey, out var userId);
            if (userId == null)
                return Task.FromResult<IdentityUser>(null);
            var user = _users.FindById(userId);
            return Task.FromResult(user);
        }
    }
}