using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain
{
    public static class Permissions
    {
        [Display(Name = "Users")]
        public static class Users
        {
            [Display(Name = "View Users", Description = "View Users Permission")]
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static IEnumerable<ApplicationPermission> GetAllPredefinedPermissions()
        {
            IEnumerable<object> permissionClasses = typeof(Permissions).GetNestedTypes(BindingFlags.Static | BindingFlags.Public).Cast<TypeInfo>();
            foreach (TypeInfo permissionClass in permissionClasses)
            {
                IEnumerable<FieldInfo> permissions = permissionClass.DeclaredFields.Where(f => f.IsLiteral);
                foreach (FieldInfo permission in permissions)
                {
                    ApplicationPermission applicationPermission = new ApplicationPermission
                    {
                        Name = permission.GetValue(null).ToString().Replace('.', ' '),
                        Value = permission.GetValue(null).ToString(),
                        Group = permissionClass.Name
                    };

                    DisplayAttribute[] attributes = (DisplayAttribute[])permission.GetCustomAttributes(typeof(DisplayAttribute), false);
                    applicationPermission.DisplayName = attributes != null && attributes.Length > 0 ? attributes[0].Name ?? applicationPermission.Name : applicationPermission.Name;
                    applicationPermission.Description = attributes != null && attributes.Length > 0 ? attributes[0].Description ?? applicationPermission.Name : applicationPermission.Name;

                    yield return applicationPermission;
                }
            }
        }
    }
}
