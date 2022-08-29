using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRoleService : IService
    {
        Task<ApplicationRole> GetByIdAsync(string id);

        Task<List<ApplicationRole>> GetAllAsync();

        Task<bool> AddOrUpdateAsync(ApplicationRole role);
    }
}
