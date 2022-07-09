using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Domain.Users;
using Dislinkt.Connections.Persistence.Neo4j.Entities;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j.Common
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly Neo4jDbContext _neo4jDbContext;

        public QueryExecutor(Neo4jDbContext neo4JDbContext)
        {
            _neo4jDbContext = neo4JDbContext;
        }

        public async Task<T> FindByIdAsync<T>(Guid id) where T : BaseEntity
            => await _neo4jDbContext.FindByIdAsync<T>(id);

        public async Task CreateAsync<T>(T t, string EntityType) where T : BaseEntity
            => await _neo4jDbContext.CreateAsync(t, EntityType);

        public async Task DeleteByIdAsync<T>(Guid id) where T : BaseEntity
            => await _neo4jDbContext.DeleteByIdAsync<T>(id);

        public async Task UpdateAsync<T>(Guid id, T t) where T : BaseEntity
            => await _neo4jDbContext.UpdateAsync(id, t);

        public async Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
            => await _neo4jDbContext.CreateConnectionAsync(sourceId, targetId, connectionName);

        public async Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
            => await _neo4jDbContext.RemoveConnectionAsync(sourceId, targetId, connectionName);

        public async Task<IReadOnlyList<Guid>> GetConnectedAsync(Guid sourceId, string connectionType)
            => await _neo4jDbContext.GetConnected(sourceId, connectionType);

        public async Task RemoveAllConnectionsAsync(Guid sourceId, Guid targetId)
            => await _neo4jDbContext.RemoveAllConnectionsAsync(sourceId, targetId);

        public async Task<IReadOnlyList<Guid>> GetConnectedReverseAsync(Guid sourceId, string connectionType)
            => await _neo4jDbContext.GetConnectedReverseAsync(sourceId, connectionType);

        public async Task<IReadOnlyList<Guid>> GetSecondLevelConnectedAsync(Guid sourceId)
            => await _neo4jDbContext.GetSecondLevelConnectedAsync(sourceId);
    }
}
