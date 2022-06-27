using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ID.eShop.Services.Identity.API.Models
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
