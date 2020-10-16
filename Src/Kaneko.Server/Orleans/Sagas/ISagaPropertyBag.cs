namespace Orleans.Sagas
{
    public interface ISagaPropertyBag
    {
        void Add<T>(string key, T value);
        T Get<T>(string key);

        void AddUserId(string userId);
        void AddUserName(string userName);
        void AddGrainId<T>(T grainId);
        void AddData<T>(T data);
        void AddOriginalData<T>(T data);
        void AddEventType(string eventType);

        string GetUserId();
        string GetUserName();
        T GetGrainId<T>();
        T GetData<T>();
        T GetOriginalData<T>();
        string GetEventType();
    }
}