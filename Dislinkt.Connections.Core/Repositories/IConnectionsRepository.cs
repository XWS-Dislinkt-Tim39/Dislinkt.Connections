﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Domain.Users;

namespace Dislinkt.Connections.Core.Repositories
{
    public interface IConnectionsRepository
    {
        Task<User> FindUserByIdAsync(Guid id);

        Task DeleteById(Guid id);
        Task CreateUser(User user);
        Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task<IReadOnlyList<Guid>> GetConnected(Guid sourceId, string connectionType);
        Task AddMessageRequestAsync(User baseUser, User receivingUser);
        Task<bool> IsFollowingAsync(User baseUser, User followedUser);
        Task<IReadOnlyList<Guid>> GetConnectedReverse(Guid sourceId, string connectionType);
        Task<IReadOnlyList<Guid>> GetSecondLevelConnectedAsync(Guid sourceId);

    }
}
