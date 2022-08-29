using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public AccountType AccountType { get; set; }
    }
}
