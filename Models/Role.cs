using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace LanfeustBridge.Models
{
    public class Role : IdentityRole
    {
        public Role()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }

        public List<string> UsersInRole { get; set; } = new List<string>();
    }
}
