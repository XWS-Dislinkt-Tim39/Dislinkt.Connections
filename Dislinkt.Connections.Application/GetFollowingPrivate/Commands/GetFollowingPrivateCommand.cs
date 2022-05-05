using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.GetFollowingPrivate.Commands
{
    public class GetFollowingPrivateCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetFollowingPrivateCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
