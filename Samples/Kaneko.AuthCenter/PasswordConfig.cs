using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaneko.AuthCenter
{
    /// <summary>
    /// 用户名密码模式
    /// </summary>
    public class PasswordConfig
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
                new ApiScope("ApoOneService"),
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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0001".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApiOneService", "ApoTwoService" }        //允许访问的api服务
                },
                new Client
                {
                    ClientId = "client2",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, //验证类型：客户端验证
                    ClientSecrets ={ new Secret("0002".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApiOneService" }        //允许访问的api服务
                },
                 new Client
                {
                    ClientId = "client3",//客户端ID                             
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials, //验证类型：用户名密码模式 和 客户端模式
                    ClientSecrets ={ new Secret("0003".Sha256())},    //密钥和加密方式
                    AllowedScopes = { "ApoTwoService" }        //允许访问的api服务
                }
            };
            return clients;
        }

        /// <summary>
        /// 定义可以使用ID4的用户资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "ypf1",     //账号
                    Password = "ypf001"    //密码
                },
                new TestUser
                {
                    SubjectId = "10002",
                    Username = "ypf2",
                    Password = "ypf002"
                },
                new TestUser
                {
                    SubjectId = "10003",
                    Username = "ypf3",
                    Password = "ypf003"
                }
            };
        }
    }
}
