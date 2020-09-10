using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Core.Contract
{
    public interface IEvent<TEventData>
    {
        string GrainId { set; get; }

        string TransactionId { set; get; }

        string EventCode { set; get; }

        TEventData Data { set; get; }
    }
}
