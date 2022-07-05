using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.GetBlocked.Commands
{
    public class GetBlockedCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetBlockedCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
