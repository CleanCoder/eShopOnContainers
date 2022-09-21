﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ID.eShop.Common.HangfireJobs.Authorization
{
    /// <summary>
    /// Represents user to access Hangfire dashboard via basic authentication
    /// </summary>
    public class BasicAuthAuthorizationUser
    {
        /// <summary>
        /// Represents user's name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// SHA1 hashed password
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        /// Setter to update password as plain text
        /// </summary>
        public string PasswordClear
        {
            set
            {
                using (var cryptoProvider = SHA1.Create())
                {
                    Password = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
                }
            }
        }

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="login">User name</param>
        /// <param name="password">User password</param>
        /// <returns></returns>
        public bool Validate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) == true)
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password) == true)
                throw new ArgumentNullException(nameof(password));

            if (login.Equals(Login, StringComparison.OrdinalIgnoreCase) == true)
            {
                using (var cryptoProvider = SHA1.Create())
                {
                    byte[] passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, Password);
                }
            }
            else
                return false;
        }
    }
}
