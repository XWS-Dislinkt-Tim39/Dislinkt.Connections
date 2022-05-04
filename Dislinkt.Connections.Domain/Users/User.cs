using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Domain.Users
{
    public class User
    {
        public User(Guid id, string userName, VisibilityStatus status)
        {
            Id = id;
            UserName = userName;
            Status = status;
        }

        public Guid Id { get; }
        public string UserName { get; }
        public VisibilityStatus Status { get; } 
    }
}
