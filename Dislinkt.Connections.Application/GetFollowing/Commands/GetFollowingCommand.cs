using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.GetFollowingPrivate.Commands
{
    public class GetFollowingCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetFollowingCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
