using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Persistence.MongoDB.Factory
{
    public interface IDatabaseFactory
    {
        IMongoDatabase Create();
    }
}
