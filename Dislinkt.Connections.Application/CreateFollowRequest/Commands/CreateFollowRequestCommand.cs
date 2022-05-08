using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.CreateFollowRequest.Commands
{
    public class CreateFollowRequestCommand : IRequest<bool>
    {
        public CreateFollowRequestCommand(ConnectionData connectionData)
        {
            Request = connectionData;
        }
        public ConnectionData Request { get; set; }
    }
}
