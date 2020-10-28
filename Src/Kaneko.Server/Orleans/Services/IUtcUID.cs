using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kaneko.Server.Orleans.Services
{
    public interface IUtcUID : IGrainWithIntegerKey
    {
        /// <summary>
        /// 通过utc时间生成分布式唯一id
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AlwaysInterleave]
        Task<string> NewID();

        /// <summary>
        /// 通过utc时间生成分布式唯一id
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AlwaysInterleave]
        Task<long> NewLongID();

        /// <summary>
        /// 批量获取多个ID
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<IEnumerable<long>> TakeNewID(int count);
    }
}
