using System.ComponentModel.DataAnnotations;

namespace ID.eShop.Services.Identity.API.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
