using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Server.Orleans.Grains
{
    public class GrainStorageKey
    {
        public const string RedisStore = "RedisStore";
        public const string MongoDBStore = "MongoDBStore";
    }
}
