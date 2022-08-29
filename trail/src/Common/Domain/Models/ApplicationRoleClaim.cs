using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public ApplicationRoleClaim() : base()
        {
        }

        public ApplicationRoleClaim(string displayName = null, string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            DisplayName = displayName;
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}
