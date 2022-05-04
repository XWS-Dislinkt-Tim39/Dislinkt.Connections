using Dislinkt.Connections.Core.Repositories;
using Dislinkt.Connections.Domain.Users;
using Dislinkt.Connections.Persistence.MongoDB.Common;
using Dislinkt.Connections.Persistence.MongoDB.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Persistence.MongoDB.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IQueryExecutor _queryExecutor;

        public UserRepository(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public async Task<User> GetById(Guid id) { 
            var result = await _queryExecutor.FindByIdAsync<UserEntity>(id);
            return result?.ToUser() ?? null;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.UserName, username);

            var result = await _queryExecutor.FindAsync(filter);

            return result?.AsEnumerable()?.FirstOrDefault(u => u.UserName == username)?.ToUser() ?? null; 
        }
    }
}
