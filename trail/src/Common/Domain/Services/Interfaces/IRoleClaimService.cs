using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRoleClaimService : IService
    {
        Task<ApplicationRoleClaim> GetByIdAsync(int id);

        Task<List<ApplicationRoleClaim>> GetAllByRoleIdAsync(string roleId, string type = "");

        Task<List<ApplicationRoleClaim>> GetAllAsync();

        Task<bool> AddOrUpdateAsync(ApplicationRoleClaim roleClaim);
    }
}
