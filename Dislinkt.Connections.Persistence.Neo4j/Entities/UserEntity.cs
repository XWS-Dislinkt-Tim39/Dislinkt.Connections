using System;
using System.Collections.Generic;
using System.Text;
using Dislinkt.Connections.Domain.Users;

namespace Dislinkt.Connections.Persistence.Neo4j.Entities
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public VisibilityStatus Status { get; set; }
        public static UserEntity ToUserEntity(User user)
            => new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                Status = user.Status
            };
        public User ToUser()
            => new User(Id, UserName, Status);
    }
}
