namespace Kaneko.Core.Contract
{
    public interface IEvent<TEventData>
    {
        string GrainId { set; get; }
        string GrainType { set; get; }

        string TransactionId { set; get; }

        TEventData Data { set; get; }
    }

    public class EventData<TEventData> : IEvent<TEventData>
    {
        public string GrainId { set; get; }

        public string GrainType { set; get; }

        /// <summary>
        /// 冥等控制
        /// </summary>
        public string TransactionId { set; get; }

        public TEventData Data { set; get; }
    }
}
