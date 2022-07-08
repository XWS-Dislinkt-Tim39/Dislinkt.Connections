using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.GetWhoBlocksMe.Commands
{
    public class GetWhoBlocksMeCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetWhoBlocksMeCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
