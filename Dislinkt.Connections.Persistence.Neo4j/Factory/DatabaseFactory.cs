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
            var uri = "neo4j+s://803184a8.databases.neo4j.io";
            var user = "neo4j";
            var password = "SiwDvvkzyx3TXwBnLCJd7-cilalOTAzMWOszLjoccxg";
            return GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }
    }
}
