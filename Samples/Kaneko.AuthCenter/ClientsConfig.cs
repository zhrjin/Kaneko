using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaneko.AuthCenter
{
    /// <summary>
    /// 客户端模式
    /// </summary>
    public class ClientsConfig
    {
        /// <summary>
        /// 配置Api范围集合
        /// 4.x版本新增的配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("ApiOneService"),
                new ApiScope("ApoTwoService")
             };
        }


        /// <summary>
        /// 需要保护的Api资源
        /// 4.x版本新增后续Scopes的配置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是ServiceName，第二个参数是描述
            resources.Add(new ApiResource("ApiOneService", "ApiOneService服务需要保护哦") { Scopes = { "ApiOneService" } });
            resources.Add(new ApiResource("ApoTwoService", "ApoTwoService服务需要保护哦") { Scopes = { "ApoTwoService" } });
            return resources;
        }

        /// <summary>
        /// 可以使用ID4 Server 客户端资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>() {
                new Client
                {
                    ClientId = "client1",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0001".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApiOneService", "ApoTwoService" }        //允许访问的api服务
                },
                new Client
                {
                    ClientId = "client2",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0002".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApiOneService" }        //允许访问的api服务
                },
                 new Client
                {
                    ClientId = "client3",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0003".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApoTwoService" }        //允许访问的api服务
                }
            };
            return clients;
        }
    }
}
