using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.CreateConnection.Commands
{
    public class CreateConnectionCommand : IRequest<bool>
    {
        public CreateConnectionCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
