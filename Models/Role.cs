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

#pragma warning disable CA2227  // setter needed for deserialization from db
        public List<string> UsersInRole { get; set; } = new List<string>();
    }
}
