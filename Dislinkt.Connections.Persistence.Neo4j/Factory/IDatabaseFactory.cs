using System;
using System.Collections.Generic;
using System.Text;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j.Factory
{
    public interface IDatabaseFactory
    {
        IDriver Create();
    }
}
