using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace LanfeustBridge.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        // use strings to help indexing
        public List<string> ExternalLogins { get; set; } = new List<string>();
    }
}