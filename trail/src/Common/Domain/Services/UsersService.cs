using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly RoleService _roleService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        
        {
            _userManager = userManager;
            _roleManager = roleManager;
           // _roleService = roleService;
           // _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetByIdAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserDTO>(user);

            return result;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = _mapper.Map<List<UserDTO>>(users);

            return result;
        }

        public async Task<List<RoleDTO>> GetRolesAsync(string userId)
        {
            // TODO:
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();

            List <RoleDTO> rolesList = new List<RoleDTO>();
            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    rolesList.Add(new RoleDTO { Id = role.Id, Name = role.Name, DisplayName = role.DisplayName, Description = role.Description });    
            }

            return rolesList;
        }

        public async Task<List<PermissionDTO>> GetPermissionAsync(string userId)
        {
            List<PermissionDTO> permissionDTOs = new List<PermissionDTO>();
            foreach(var role in await GetRolesAsync(userId))
            {
                var permissions = await _roleService.GetAllPermissionsAsync(role.Id);
                permissionDTOs.AddRange(_mapper.Map<List<PermissionDTO>>(permissions));
            }

            return permissionDTOs;
        }
    }
}
