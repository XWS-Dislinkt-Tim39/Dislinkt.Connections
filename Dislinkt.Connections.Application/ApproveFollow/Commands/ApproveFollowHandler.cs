using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.ApproveFollow.Commands
{
    public class ApproveFollowHandler : IRequestHandler<ApproveFollowCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public ApproveFollowHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(ApproveFollowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionsRepository.RemoveConnectionAsync(targetId, sourceId, "FOLLOW_REQUEST");
                await _connectionsRepository.CreateConnectionAsync(sourceId, targetId, "FOLLOWS");
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
