using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.Follow.Commands
{
    public class FollowCommand : IRequest<bool>
    {

        public FollowCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }

    }
}
