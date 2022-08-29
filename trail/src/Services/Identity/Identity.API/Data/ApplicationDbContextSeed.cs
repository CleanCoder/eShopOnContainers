using Domain.Models;
using ID.eShop.Services.Identity.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ID.eShop.Services.Identity.API.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env, 
            ILogger<ApplicationDbContextSeed> logger, IOptions<AppSettings> settings, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    context.Users.AddRange(GetDefaultUser());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, settings, retryForAvaiability);
                }
            }
        }


        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user = new ApplicationUser()
            {
                Email = "abc@gmail.com",
                Id = Guid.NewGuid().ToString(),
                UserName = "abc@gmail.com",
                NormalizedEmail = "ABC@GMAIL.COM",
                NormalizedUserName = "ABC@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                AccountStatus = AccountStatus.Active,
                AccountType = AccountType.Admin,
                EmailConfirmed = true
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            return new List<ApplicationUser>(){user };
        }
    }
}
