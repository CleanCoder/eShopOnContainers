using ID.eShop.Services.Identity.API.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.CustomTokenProviders
{
    public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : IdentityUser
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly EmailConfirmationTokenProviderOptions _option;


        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<EmailConfirmationTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger,
            IVerificationCodeService codeService)
            : base(dataProtectionProvider, options, logger)
        {
            _verificationCodeService = codeService;
            _option = options.Value;
        }

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var verificationCode = await _verificationCodeService.GetCodeAsync(user.Id);
            if (verificationCode == null)
            {
                verificationCode = new Models.UserVerificationCode
                {
                    UserId = user.Id,
                    Purpose = purpose,
                 
                    SecurityStamp = Guid.NewGuid().ToString()
                };
            }

            verificationCode.Nonce = GenerateVerificationCode();
            verificationCode.TriesLeft = _option.MaxAttempt;
            verificationCode.ExpBefore = DateTime.UtcNow.Add(_option.TokenLifespan);

            await _verificationCodeService.AddOrUpdateCodeAsync(verificationCode);

            return verificationCode.Nonce;
        }

        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var verificationCode = await _verificationCodeService.GetCodeAsync(user.Id);
            if (verificationCode == null)
                return false;

            if (verificationCode.Nonce != token)
            {
                verificationCode.TriesLeft--;
                await _verificationCodeService.AddOrUpdateCodeAsync(verificationCode);
                return false;
            }

            if (DateTimeOffset.UtcNow < verificationCode.ExpBefore && verificationCode.TriesLeft > 0)
            {
                verificationCode.Nonce = "";
                await _verificationCodeService.AddOrUpdateCodeAsync(verificationCode);

                return true;
            }

            return false;
        }


        public string GenerateVerificationCode()
        {
            return RandomNumberGenerator.GetInt32(1000, 9999).ToString("D4");
        }
    }

    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public EmailConfirmationTokenProviderOptions()
        {
            TokenLifespan = TimeSpan.FromMinutes(15);
        }

        public int MaxAttempt { get; set; } = 5;
    } 
}
