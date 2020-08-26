using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Kaneko.Core.Orleans.Grains
{
    public interface IRemindableGrain : IRemindable, IGrainWithStringKey
    {
        Task<IGrainReminder> StartReminder(TimeSpan? timeSpan = null);

        Task RemoveReminder();

        Task ReceiveReminder();

        /// <summary>
        /// 定时时间（分钟）
        /// </summary>
        Task<TimeSpan> GetReminderUsePeriod();

        /// <summary>
        /// 提醒名称
        /// </summary>
        Task<string> GetReminderName();
    }
}
