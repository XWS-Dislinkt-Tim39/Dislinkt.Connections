using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Persistence.Neo4j.Entities;

namespace Dislinkt.Connections.Persistence.Neo4j.Common
{
    public interface IQueryExecutor
    {
        Task<T> FindByIdAsync<T>(Guid id) where T : BaseEntity;
        Task CreateAsync<T>(T t, string EntityType) where T : BaseEntity;
        Task DeleteByIdAsync<T>(Guid id) where T : BaseEntity;
        Task UpdateAsync<T>(Guid id, T t) where T : BaseEntity;
        Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task<IReadOnlyList<Guid>> GetConnectedAsync(Guid sourceId, string connectionType);

    }
}
