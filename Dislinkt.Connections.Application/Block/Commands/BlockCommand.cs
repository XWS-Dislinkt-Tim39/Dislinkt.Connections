using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.Block.Commands
{
    public class BlockCommand : IRequest<bool>
    {
        public BlockCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
