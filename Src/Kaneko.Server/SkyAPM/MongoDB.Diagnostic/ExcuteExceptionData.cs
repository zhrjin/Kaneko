using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Server.SkyAPM.MongoDB.Diagnostic
{
    public class ExcuteExceptionData : ExcuteData
    {
        public Exception Ex { get; }


        public ExcuteExceptionData(Guid operationId, string operation, MongoClient mongoClient, Exception ex) : base(operationId, operation, mongoClient)
        {
            Ex = ex;
        }
    }
}
