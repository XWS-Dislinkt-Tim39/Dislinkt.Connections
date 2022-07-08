using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.GetWhoBlocksMe.Commands
{
    public class GetWhoBlocksMeHandler : IRequestHandler<GetWhoBlocksMeCommand, IReadOnlyList<Guid>>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public GetWhoBlocksMeHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<IReadOnlyList<Guid>> Handle(GetWhoBlocksMeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _connectionsRepository.GetConnectedReverse(request.SourceId, "BLOCKS");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
