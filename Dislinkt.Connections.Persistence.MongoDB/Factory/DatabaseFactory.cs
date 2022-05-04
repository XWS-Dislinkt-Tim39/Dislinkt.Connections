using MongoDB.Driver;

namespace Dislinkt.Connections.Persistence.MongoDB.Factory
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public IMongoDatabase Create()
        {
            MongoClient mongoClient = new MongoClient("mongodb+srv://aleksandramitro:SifrazaMongo99!@cluster0.qmuvt.mongodb.net/xml?retryWrites=true&w=majority");
            return mongoClient.GetDatabase("xml");
        }
    }
}
