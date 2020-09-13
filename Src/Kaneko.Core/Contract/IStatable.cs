namespace Kaneko.Core.Contract
{
    public interface IStatable<T> where T : IState
    {
        ProcessAction GetAction();

        T GetState();
    }

    public enum ProcessAction
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create,

        /// <summary>
        /// 修改
        /// </summary>
        Update,

        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }

    public class Statable<T> : IStatable<T> where T : IState
    {
        private T State { set; get; }
        private ProcessAction StateAction { set; get; }

        public Statable(ProcessAction action, T state = default)
        {
            State = state;
            StateAction = action;
        }

        public ProcessAction GetAction()
        {
            return StateAction;
        }

        public T GetState()
        {
            return State;
        }
    }
}
