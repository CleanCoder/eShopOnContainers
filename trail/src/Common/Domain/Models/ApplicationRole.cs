using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
        }

        public ApplicationRole(string roleName, string displayName = null,  string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();

            DisplayName = displayName;
            Description = roleDescription;
        }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
