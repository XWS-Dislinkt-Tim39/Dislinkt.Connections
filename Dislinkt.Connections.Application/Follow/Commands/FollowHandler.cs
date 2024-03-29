﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.Follow.Commands
{
    public class FollowHandler : IRequestHandler<FollowCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public FollowHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionsRepository.CreateConnectionAsync(sourceId, targetId, "FOLLOWS");
                await _connectionsRepository.CreateConnectionAsync(targetId, sourceId, "FOLLOWS");
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
