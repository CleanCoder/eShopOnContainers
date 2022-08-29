using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ApplicationPermission
    {
        public int Id { get; set; }

        public string RoleId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
    }
}
