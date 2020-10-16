using SkyApm.Config;
using SkyApm.Tracing;
using System.Collections;
using System.Collections.Generic;

namespace Kaneko.Server.SkyAPM
{
    public class HttpRequestCarrierHeaderCollection : ICarrierHeaderCollection
    {
        private readonly IEnumerable<KeyValuePair<string, string>> _headers;

        public HttpRequestCarrierHeaderCollection(string sw8)
        {
            _headers = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(HeaderVersions.SW8, sw8) };
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _headers.GetEnumerator();
        }

        public void Add(string key, string value)
        {

        }
    }
}
