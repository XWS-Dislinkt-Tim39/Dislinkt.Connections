using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.Unblock.Commands
{
    public class UnblockCommand : IRequest<bool>
    {
        public UnblockCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
