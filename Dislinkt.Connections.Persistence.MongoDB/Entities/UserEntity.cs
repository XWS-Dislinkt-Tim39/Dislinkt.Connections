using System;
using System.Collections.Generic;
using System.Text;
using Dislinkt.Connections.Domain.Users;
using Dislinkt.Connections.Persistence.MongoDB.Attributes;

namespace Dislinkt.Connections.Persistence.MongoDB.Entities
{
    [CollectionName("Users")]
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public VisibilityStatus Status { get; set; }

        public UserEntity(){}
        public UserEntity(string username, VisibilityStatus status)
        {
            UserName = username;
            Status = status;
        }

        public static UserEntity ToUserEntity(User user)
            => new UserEntity
            {
                UserName = user.UserName,
                Status = user.Status
            };
        public User ToUser()
            => new User(this.Id, this.UserName, this.Status);
    }
}
