using Dislinkt.Connections.Persistence.MongoDB.Attributes;
using Dislinkt.Connections.Persistence.MongoDB.Entities;
using Dislinkt.Connections.Persistence.MongoDB.Factory;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Persistence.MongoDB.Common
{
    public class MongoDbContext
    {
        private readonly IDatabaseFactory _databaseFactory;

        public MongoDbContext(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public IMongoCollection<T> GetCollection<T>() where T : BaseEntity
        {
            CollectionNameAttribute collectionDefinition = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute)) as CollectionNameAttribute;

            return GetCollection<T>(collectionDefinition.Name);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
            => _databaseFactory.Create().GetCollection<T>(collectionName);

        public async Task CreateCollection<T>() where T : BaseEntity
        {
            CollectionNameAttribute collectionDefinition = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute)) as CollectionNameAttribute;

            IMongoDatabase db = _databaseFactory.Create();
            List<string> collections = (await db.ListCollectionNamesAsync()).ToList();

            if(collections.Any(c => c == collectionDefinition.Name)){
                return;
            }

            await _databaseFactory.Create().CreateCollectionAsync(collectionDefinition.Name);
;        }
    }
}
