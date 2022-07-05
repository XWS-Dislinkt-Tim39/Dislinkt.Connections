using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.GetBlocked.Commands
{
    public class GetBlockedHandler : IRequestHandler<GetBlockedCommand, IReadOnlyList<Guid>>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public GetBlockedHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<IReadOnlyList<Guid>> Handle(GetBlockedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _connectionsRepository.GetConnected(request.SourceId, "BLOCKS");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
