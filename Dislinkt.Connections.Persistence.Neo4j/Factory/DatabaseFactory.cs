using System;
using System.Collections.Generic;
using System.Text;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j.Factory
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public IDriver Create()
        {
            var uri = "bolt://localhost:7687";
            var user = "neo4j";
            var password = "dislinkt";
            return GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }
    }
}
