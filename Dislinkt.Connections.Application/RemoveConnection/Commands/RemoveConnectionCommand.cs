using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.RemoveConnection.Commands
{
    public class RemoveConnectionCommand : IRequest<bool>
    {
        public RemoveConnectionCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }

        public ConnectionData Request { get; set; }
    }
}
