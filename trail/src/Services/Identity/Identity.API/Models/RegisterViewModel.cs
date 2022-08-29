using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ID.eShop.Services.Identity.API.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        public AccountType AccountType { get; set; } = AccountType.Visitor;


        public ApplicationUser User { get; set; }
    }
}
