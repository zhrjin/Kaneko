using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Kaneko.Core.Orleans.Grains
{
    /// <summary>
    /// 提醒+状态Grain
    /// </summary>
    /// <typeparam name="PrimaryKey"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class ReminderStateGrain<PrimaryKey, TState> : StateGrain<PrimaryKey, TState>, IRemindableGrain where TState : new()
    {
        /// <summary>
        /// 异步获取提醒名称
        /// </summary>
        public Task<string> GetReminderName() => new ValueTask<string>(ReminderName).AsTask();

        /// <summary>
        /// 异步获取定时时间（分钟）
        /// </summary>
        public Task<TimeSpan> GetReminderUsePeriod() => new ValueTask<TimeSpan>(ReminderUsePeriod).AsTask();

        /// <summary>
        /// 提醒名称
        /// </summary>
        public abstract string ReminderName { get; }

        /// <summary>
        /// 定时时间（分钟）
        /// </summary>
        public abstract TimeSpan ReminderUsePeriod { get; }

        public abstract Task ReceiveReminder();

        public async Task RemoveReminder()
        {
            var reminderType = await GetReminder(ReminderName);
            if (reminderType != null)
                await UnregisterReminder(reminderType);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public async Task<IGrainReminder> StartReminder(TimeSpan? timeSpan = null)
        {
            TimeSpan usePeriod = ReminderUsePeriod;
            if (timeSpan != null)
            {
                usePeriod = timeSpan.Value;
            }
            IGrainReminder gr = await RegisterOrUpdateReminder(ReminderName, usePeriod - TimeSpan.FromSeconds(2), usePeriod);
            return gr;
        }

        /// <summary>
        /// 消息接收
        /// </summary>
        /// <param name="reminderName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            await ReceiveReminder();
        }
    }
}
