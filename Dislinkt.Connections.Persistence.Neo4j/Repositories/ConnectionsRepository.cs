using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using Dislinkt.Connections.Domain.Users;
using Dislinkt.Connections.Persistence.Neo4j.Common;
using Dislinkt.Connections.Persistence.Neo4j.Entities;
using Neo4j.Driver;

namespace Dislinkt.Connections.Persistence.Neo4j.Repositories
{
    public class ConnectionsRepository : IConnectionsRepository
    {
        private readonly IQueryExecutor _queryExecutor;
        public ConnectionsRepository(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public async Task<User> FindUserByIdAsync(Guid id)
        {
            UserEntity result = await _queryExecutor.FindByIdAsync<UserEntity>(id);
            if (result == null)
            {
                Trace.WriteLine("User not found in graph database.");
                return null;
            }

            User user = result.ToUser();
            return user;
        }

        public async Task CreateUser(User user)
        {
            UserEntity entity = UserEntity.ToUserEntity(user);
            await _queryExecutor.CreateAsync(entity, "USER");
        }

        public async Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            await _queryExecutor.CreateConnectionAsync(sourceId, targetId, connectionName);
            await _queryExecutor.CreateConnectionAsync(targetId, sourceId, connectionName);
        }
        public async Task CreateConnectionRequestAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            await _queryExecutor.CreateConnectionAsync(sourceId, targetId, connectionName);
        }
        public async Task RemoveConnectionRequestAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            await _queryExecutor.RemoveConnectionAsync(sourceId, targetId, connectionName);
        }

        public async Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName)
        {
            await _queryExecutor.RemoveConnectionAsync(sourceId, targetId, connectionName);
            await _queryExecutor.RemoveConnectionAsync(targetId, sourceId, connectionName);
        }

        public async Task<IReadOnlyList<Guid>> GetConnected(Guid sourceId, string connectionType)
        {
            return await _queryExecutor.GetConnectedAsync(sourceId, connectionType);
        }

        public async Task BlockUserAsync(User blockingUser, User blockedUser)
        {

        }

        public async Task AddMessageRequestAsync(User baseUser, User receivingUser)
        {

        }

        public async Task<bool> IsFollowingAsync(User baseUser, User followedUser)
        {


            return false;
        }
    }
}
