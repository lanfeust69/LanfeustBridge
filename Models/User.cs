using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace LanfeustBridge.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = default!;

        // use strings to help indexing
        public List<string> ExternalLogins { get; set; } = new List<string>();

        public bool IsTwoFactorEnabled { get; set; }

        public string? AuthenticatorKey { get; set; }

        public List<string> RecoveryCodes { get; set; } = new List<string>();

        public List<string> Roles { get; set; } = new List<string>();
    }
}
