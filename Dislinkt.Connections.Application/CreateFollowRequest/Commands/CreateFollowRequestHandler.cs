using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.CreateFollowRequest.Commands
{
    public class CreateFollowRequestHandler : IRequestHandler<CreateFollowRequestCommand, bool>
    {
        private readonly IConnectionsRepository _connectionRepository;

        public CreateFollowRequestHandler(IConnectionsRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public async Task<bool> Handle(CreateFollowRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionRepository.CreateConnectionAsync(sourceId, targetId, "FOLLOW_REQUEST");
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
