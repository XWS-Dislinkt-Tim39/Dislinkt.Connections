using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dislinkt.Connections.Domain.Users;

namespace Dislinkt.Connections.Core.Repositories
{
    public interface IConnectionsRepository
    {
        Task<User> FindUserByIdAsync(Guid id);
        Task CreateUser(User user);
        Task CreateConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task RemoveConnectionAsync(Guid sourceId, Guid targetId, string connectionName);
        Task<IReadOnlyList<Guid>> GetConnected(Guid sourceId, string connectionType);
        Task BlockUserAsync(User blockingUser, User blockedUser);
        Task AddMessageRequestAsync(User baseUser, User receivingUser);
        Task<bool> IsFollowingAsync(User baseUser, User followedUser);

    }
}
