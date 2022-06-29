using ID.eShop.Services.Identity.API.Data;
using ID.eShop.Services.Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Services
{
    public interface IVerificationCodeService
    {
        Task<UserVerificationCode> GetCodeAsync(string userId);

        Task<bool> AddOrUpdateCodeAsync(UserVerificationCode verificationCode);
    }

    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ApplicationDbContext _context;

        public VerificationCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<UserVerificationCode> GetCodeAsync(string userId)
        {
            return _context.VerificationCodes.SingleOrDefaultAsync(item => item.UserId == userId);
        }

        public async Task<bool> AddOrUpdateCodeAsync(UserVerificationCode verificationCode)
        {
            if (verificationCode == null)
                throw new ArgumentNullException(nameof(verificationCode));

             _context.VerificationCodes.Update(verificationCode);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
