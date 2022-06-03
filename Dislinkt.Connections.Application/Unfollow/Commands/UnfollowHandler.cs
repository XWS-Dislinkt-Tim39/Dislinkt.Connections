using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.Unfollow.Commands
{
    public class UnfollowHandler : IRequestHandler<UnfollowCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public UnfollowHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionsRepository.RemoveConnectionAsync(sourceId, targetId, "FOLLOWS");
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
