using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.RejectFollow.Commands
{
    public class RejectFollowHandler : IRequestHandler<RejectFollowCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public RejectFollowHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(RejectFollowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionsRepository.RemoveConnectionAsync(targetId, sourceId, "FOLLOW_REQUEST");
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
