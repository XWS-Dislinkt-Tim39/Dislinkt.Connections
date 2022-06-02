using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.Unfollow.Commands
{
    public class UnfollowCommand : IRequest<bool>
    {
        public UnfollowCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
