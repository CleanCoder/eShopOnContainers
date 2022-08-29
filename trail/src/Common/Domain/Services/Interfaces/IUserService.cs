using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IUserService : IService
    {
        Task<UserDTO> GetByIdAsync(string userId);

        Task<List<UserDTO>> GetAllAsync();

        Task<List<RoleDTO>> GetRolesAsync(string userId);

        Task<List<PermissionDTO>> GetPermissionAsync(string userId);
    }
}
