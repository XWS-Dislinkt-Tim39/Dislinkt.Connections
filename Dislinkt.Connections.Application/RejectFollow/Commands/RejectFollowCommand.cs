using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.RejectFollow.Commands
{
    public class RejectFollowCommand : IRequest<bool>
    {

        public RejectFollowCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
