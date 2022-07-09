using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Application.GetFollowRecommendations.Commands
{
    public class GetFollowRecommendationsCommand : IRequest<IReadOnlyList<Guid>>
    {
        public GetFollowRecommendationsCommand(Guid sourceId)
        {
            SourceId = sourceId;
        }
        public Guid SourceId { get; set; }
    }
}
