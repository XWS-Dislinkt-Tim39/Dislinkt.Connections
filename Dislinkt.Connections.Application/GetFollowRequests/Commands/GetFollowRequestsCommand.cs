using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.GetFollowRequests.Commands
{
    public class GetFollowRequestsCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetFollowRequestsCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
