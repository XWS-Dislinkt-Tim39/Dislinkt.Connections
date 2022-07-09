using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.GetFollowRecommendations.Commands
{
    public class GetFollowRecommendationsHandler : IRequestHandler<GetFollowRecommendationsCommand, IReadOnlyList<Guid>>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public GetFollowRecommendationsHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<IReadOnlyList<Guid>> Handle(GetFollowRecommendationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _connectionsRepository.GetSecondLevelConnectedAsync(request.SourceId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
