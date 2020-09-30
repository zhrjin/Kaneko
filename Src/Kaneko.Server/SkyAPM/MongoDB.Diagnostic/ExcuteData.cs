using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Server.SkyAPM.MongoDB.Diagnostic
{
    public class ExcuteData
    {
        public ExcuteData(Guid operationId, string operation, MongoClient mongoClient)
        {
            OperationId = operationId;
            Operation = operation;
            MongoClient = mongoClient;
        }
        public Guid OperationId { get; }

        public string Operation { get; }

        public MongoClient MongoClient { get; }
    }
}
