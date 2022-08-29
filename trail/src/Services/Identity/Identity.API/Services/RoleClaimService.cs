using Domain.Models;
using Domain.Services;
using ID.eShop.Services.Identity.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Services
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleClaimService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationRoleClaim> GetByIdAsync(int id)
        {
            var roleClaim = await _dbContext.RoleClaims.SingleOrDefaultAsync(x => x.Id == id);

            return roleClaim;
        }

        public async Task<List<ApplicationRoleClaim>> GetAllByRoleIdAsync(string roleId, string claimType = "")
        {
            var query = _dbContext.RoleClaims.Include(x => x.Role).Where(x => x.RoleId == roleId);
            if (!string.IsNullOrEmpty(claimType))
                query = query.Where(x => x.ClaimType == claimType);

            var roleClaims = await query.ToListAsync();

            return roleClaims;
        }

        public async Task<bool> AddOrUpdateAsync(ApplicationRoleClaim roleClaim)
        {
            if (string.IsNullOrWhiteSpace(roleClaim.RoleId))
                throw new ArgumentNullException(nameof(roleClaim.RoleId));

            if (roleClaim.Id == 0)
            {
                var existingRoleClaim = await _dbContext.RoleClaims.SingleOrDefaultAsync(x => x.RoleId == roleClaim.RoleId && x.ClaimType == roleClaim.ClaimType && x.ClaimValue == roleClaim.ClaimValue);
                if (existingRoleClaim != null)
                    return false; // Throw BusinessExistException,  "Similar Role Claim already exists."

                await _dbContext.RoleClaims.AddAsync(roleClaim);
                //await _dbContext.SaveChangesAsync(_currentUserService.UserId);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                var existingRoleClaim = await _dbContext.RoleClaims.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == roleClaim.Id);
                if (existingRoleClaim == null)
                    return false; // Throw BusinessNotFoundException,  "Role Claim does not exist."
                
                existingRoleClaim.RoleId = roleClaim.RoleId;
                existingRoleClaim.ClaimType = roleClaim.ClaimType;
                existingRoleClaim.ClaimValue = roleClaim.ClaimValue;
                existingRoleClaim.DisplayName = roleClaim.DisplayName;
                existingRoleClaim.Group = roleClaim.Group;
                existingRoleClaim.Description = roleClaim.Description;
               
                _dbContext.RoleClaims.Update(existingRoleClaim);
                //await _dbContext.SaveChangesAsync(_currentUserService.UserId);

                return await _dbContext.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<ApplicationRoleClaim>> GetAllAsync()
        {
            var roleClaims = await _dbContext.RoleClaims.ToListAsync();

            return roleClaims;
        }
    }
}
