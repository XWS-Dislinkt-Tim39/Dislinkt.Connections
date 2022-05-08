using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.GetFollowRequests.Commands
{
    public class GetFollowRequestsHandler : IRequestHandler<GetFollowRequestsCommand, IReadOnlyList<Guid>>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public GetFollowRequestsHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<IReadOnlyList<Guid>> Handle(GetFollowRequestsCommand request,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<Guid> retVal = new List<Guid>();
            try
            {
                retVal = await _connectionsRepository.GetConnected(request.SourceId, "FOLLOW_REQUEST");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }

            return retVal;
        }
    }
}
