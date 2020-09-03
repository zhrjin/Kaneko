using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kaneko.IdentityCenter.Configurations
{
    internal class Config
    {
        static IConfigurationRoot _configuration;
        static Config()
        {
            string dir = Directory.GetCurrentDirectory();
            _configuration = new ConfigurationBuilder().AddJsonFile("id4.json")
                .SetBasePath(dir)
                .Build();
        }
        internal static IEnumerable<Client> GetClients()
        {
            var clients = _configuration.GetSection("Clients").Get<List<Client>>();
            foreach (Client client in clients)
            {
                foreach (var secret in client.ClientSecrets)
                {
                    secret.Value = secret.Value.Sha256();
                }
            }
            return clients;
        }
        internal static IEnumerable<ApiScope> GetApiScopes()
        {
            var scopes = _configuration.GetSection("ApiScopes").Get<List<ApiScope>>();
            return scopes;
        }
        internal static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

    }





}
