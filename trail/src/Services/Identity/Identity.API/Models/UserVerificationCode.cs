using System;
using System.ComponentModel.DataAnnotations;

namespace ID.eShop.Services.Identity.API.Models
{
    public class UserVerificationCode
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string Nonce { get; set; }

        [MaxLength(length: 100)]
        public string SecurityStamp { get; set; }

        public int TriesLeft { get; set; }

        public DateTimeOffset? ExpBefore { get; set; }

        [MaxLength(length:50)]
        public string Purpose { get; set; }
    }
}
