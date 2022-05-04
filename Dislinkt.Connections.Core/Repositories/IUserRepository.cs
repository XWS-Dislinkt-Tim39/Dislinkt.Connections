
using Dislinkt.Connections.Domain.Users;
using System;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> GetByUsernameAsync(string username);
    }
}
