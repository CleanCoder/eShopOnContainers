using Domain.Constants;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleClaimService _roleClaimService;

        public RoleService(
            UserManager<ApplicationUser> userManager,
             IRoleClaimService roleClaimService)
        {
            //_roleManager = roleManager;
            _userManager = userManager;
            _roleClaimService = roleClaimService;
        }

        public async Task<ApplicationRole> GetByIdAsync(string id)
        {
            var role = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);

            return role;
        }

        public async Task<List<ApplicationRole>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles;
        }

        public async Task<bool> AddOrUpdateAsync(ApplicationRole role)
        {
            if (string.IsNullOrEmpty(role.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(role.Name);
                if (existingRole != null)
                    throw new ArgumentException("Similar Role already exists.");

                var result = await _roleManager.CreateAsync(role);
                 
                return result.Succeeded;
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(role.Id);
                var result = await _roleManager.UpdateAsync(existingRole);

                return result.Succeeded;
            }
        }

        public async Task<List<ApplicationPermission>> GetAllPermissionsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return new List<ApplicationPermission>();

            var permissions = new List<ApplicationPermission>();
            var allPermissions = Permissions.GetAllPredefinedPermissions().ToList();

            var roleClaims = await _roleClaimService.GetAllByRoleIdAsync(role.Id, ApplicationClaimTypes.Permission);
            foreach(var claim in roleClaims)
            {
                // TODO: 
                var matchedPermission = allPermissions.SingleOrDefault(item => item.Value == claim.ClaimValue);

                permissions.Add(new ApplicationPermission
                {
                    RoleId = roleId,
                    Name = claim.ClaimValue,
                    DisplayName = claim.DisplayName,
                    Group = claim.Group,
                    Description = claim.Description
                });
            }

            return permissions;
        }
    }
}
