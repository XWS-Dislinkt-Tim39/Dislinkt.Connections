using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.RemoveConnection.Commands
{
    public class RemoveConnectionHandler : IRequestHandler<RemoveConnectionCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public RemoveConnectionHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(RemoveConnectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid targetId = Guid.Parse(request.Request.TargetId);
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                await _connectionsRepository.RemoveConnectionAsync(sourceId, targetId, request.Request.ConnectionName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }
    }
}
