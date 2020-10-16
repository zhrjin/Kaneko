using Newtonsoft.Json;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    class SagaPropertyBag : ISagaPropertyBag
    {
        private readonly Dictionary<string, string> existingProperties;

        public Dictionary<string, string> ContextProperties { get; }

        public SagaPropertyBag() : this(new Dictionary<string, string>())
        {
        }

        public SagaPropertyBag(Dictionary<string, string> existingProperties)
        {
            this.existingProperties = existingProperties;
            ContextProperties = new Dictionary<string, string>();
        }

        public void Add<T>(string key, T value)
        {
            if (typeof(T) == typeof(string))
            {
                ContextProperties.Add(key, (string)(dynamic)value);
                return;
            }

            ContextProperties.Add(key, JsonConvert.SerializeObject(value));
        }

        public T Get<T>(string key)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(dynamic)existingProperties[key];
            }

            return JsonConvert.DeserializeObject<T>(existingProperties[key]);
        }

        public void AddUserId(string userId)
        {
            Add(USERID_KEY, userId);
        }

        public void AddUserName(string userName)
        {
            Add(USERNAME_KEY, userName);
        }

        public void AddGrainId<T>(T grainId)
        {
            Add(GRAINID_KEY, grainId);
        }

        public void AddData<T>(T data)
        {
            Add(DATE_KEY, data);
        }

        public void AddOriginalData<T>(T data)
        {
            Add(ORIGINAL_VALUE, data);
        }

        public void AddEventType(string eventType)
        {
            Add(EVENT_TYPE, eventType);
        }

        public string GetUserId()
        {
            return Get<string>(USERID_KEY);
        }

        public string GetUserName()
        {
            return Get<string>(USERNAME_KEY);
        }

        public T GetGrainId<T>()
        {
            return Get<T>(GRAINID_KEY);
        }

        public T GetData<T>()
        {
            return Get<T>(DATE_KEY);
        }

        public T GetOriginalData<T>()
        {
            return Get<T>(ORIGINAL_VALUE);
        }

        public string GetEventType()
        {
            return Get<string>(EVENT_TYPE);
        }

        private const string USERID_KEY = "SAGA_USERID_KEY";
        private const string USERNAME_KEY = "SAGA_USERNAME_KEY";
        private const string GRAINID_KEY = "SAGA_GRAINID_KEY";
        private const string DATE_KEY = "SAGA_DATE_KEY";
        private const string ORIGINAL_VALUE = "SAGA_ORIGINAL_VALUE_KEY";
        private const string EVENT_TYPE = "SAGA_EVENT_TYPE_KEY";
    }
}
