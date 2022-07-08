using Dislinkt.Connections.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dislinkt.Connections.Application.Block.Commands
{
    public class BlockHandler : IRequestHandler<BlockCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;
        public BlockHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }
        public async Task<bool> Handle(BlockCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionsRepository.CreateConnectionAsync(sourceId, targetId, "BLOCKS");
                await _connectionsRepository.RemoveConnectionAsync(sourceId, targetId, "FOLLOWS");
                await _connectionsRepository.RemoveConnectionAsync(targetId, sourceId, "FOLLOWS");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
    }
}
