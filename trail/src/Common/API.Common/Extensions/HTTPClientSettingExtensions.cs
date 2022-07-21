using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ID.eShop.API.Common.Extensions
{
    public static class HTTPClientSettingExtensions
    {
        public static T GetSectionAs<T>(this IConfiguration configuration, string sectionName) where T : HttpClientSettings, new()
        {
            var setting = new T();
            
            try
            {
                configuration.GetSection(sectionName).Bind(setting);
                return setting;

                //var clientSetting = JsonSerializer.Deserialize<T>(setting);
                //return clientSetting;
            }
            catch (Exception)
            { }

            return new T();
        }
    }
}
