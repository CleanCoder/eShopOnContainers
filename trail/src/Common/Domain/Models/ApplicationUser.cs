using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public AccountStatus AccountStatus { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
    }

    public enum AccountStatus
    {
        Active,
        InActive
    }

    public enum AccountType
    {
        Visitor,
        Admin
    }
}
