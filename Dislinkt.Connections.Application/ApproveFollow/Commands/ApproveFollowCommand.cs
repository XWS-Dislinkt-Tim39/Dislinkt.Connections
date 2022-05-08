using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.ApproveFollow.Commands
{
    public class ApproveFollowCommand : IRequest<bool>
    {
        public ApproveFollowCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
