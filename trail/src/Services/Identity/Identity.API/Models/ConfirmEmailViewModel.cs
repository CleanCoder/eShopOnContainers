using System.ComponentModel.DataAnnotations;

namespace ID.eShop.Services.Identity.API.Models
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        public string SecurityStamp { get; set; }

        public string ReturnUrl { get; set; }
    }
}
