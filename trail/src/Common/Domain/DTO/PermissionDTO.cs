using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PermissionDTO
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Group { get; set; }

        public string Description { get; set; }
    }
}
