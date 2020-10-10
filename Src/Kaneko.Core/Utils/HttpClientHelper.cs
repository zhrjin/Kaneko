using ICSharpCode.SharpZipLib.GZip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kaneko.Core.Utils
{
    /// <summary>
    /// http 请求服务
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 使用post方法异步请求 
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="posData">发送的参数JSON字符串</param>
        /// <param name="header">请求头</param>
        /// <param name="posFrom">表单提交格式</param>
        /// <param name="gzip">是否压缩</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostKanekoAsync(this HttpClient client, string url, string posData, Dictionary<string, string> header = null, Dictionary<string, string> posFrom = null, bool gzip = false)
        {
            //消息状态
            string responseBody = string.Empty;
            //存在则是表单提交信息
            if (posFrom != null)
            {
                var formData = new MultipartFormDataContent();
                foreach (var item in posFrom)
                {
                    formData.Add(new StringContent(item.Value), item.Key);
                }
                //提交信息
                var result = await client.PostAsync(url, formData);
                //获取消息状态
                responseBody = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            }
            else
            {//json
                HttpContent content = new StringContent(posData);
                if (header != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    foreach (var item in header)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                if (gzip)
                {
                    GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                    responseBody = new StreamReader(inputStream).ReadToEnd();
                }
                else
                {
                    responseBody = await response.Content.ReadAsStringAsync();

                }
            }

            return responseBody;

        }

        /// <summary>
        /// 使用get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="header"></param>
        /// <param name="Gzip"></param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> GetKanekoAsync(this HttpClient client, string url, Dictionary<string, string> header = null, bool Gzip = false)
        {
            //HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();//用来抛异常
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            return responseBody;
        }

        /// <summary>
        /// 使用post返回异步请求直接返回对象
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <typeparam name="T2">请求对象类型</typeparam>
        /// <param name="url">请求链接</param>
        /// <param name="obj">请求对象数据</param>
        /// <param name="header">请求头</param>
        /// <param name="postFrom">表单提交 表单提交 注* postFrom不为null 代表表单提交, 为null标识惊悚格式请求</param>
        /// <param name="gzip">是否压缩</param>
        /// <returns>请求返回的目标对象</returns>
        public static async Task<T> PostObjectAsync<T, T2>(this HttpClient client, string url, T2 obj, Dictionary<string, string> header = null, Dictionary<string, string> postFrom = null, bool gzip = false)
        {
            String json = JsonConvert.SerializeObject(obj);
            string responseBody = await PostKanekoAsync(client, url, json, header, postFrom, gzip); //请求当前账户的信息
            return JsonConvert.DeserializeObject<T>(responseBody);//把收到的字符串序列化
        }

        /// <summary>
        /// 使用Get返回异步请求直接返回对象
        /// </summary>
        /// <typeparam name="T">请求对象类型</typeparam>
        /// <param name="url">请求链接</param>
        /// <returns>返回请求的对象</returns>
        public static async Task<T> GetObjectAsync<T>(this HttpClient client, string url)
        {
            string responseBody = await GetKanekoAsync(client, url); //请求当前账户的信息
            return JsonConvert.DeserializeObject<T>(responseBody);//把收到的字符串序列化
        }

    }
}
