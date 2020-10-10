using Kaneko.Core.Contract;
using System.Collections.Generic;

namespace YTSoft.BasicData.IGrains.DataDictionary.Model
{
    public class DictState : IState
    {
        public GrainDataState GrainDataState { get; set; }

        public List<DictDO> Balance { set; get; } = new List<DictDO>();
    }
}
