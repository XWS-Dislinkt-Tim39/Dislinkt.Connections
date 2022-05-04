using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.CreateConnection.Commands
{
    public class CreateConnectionHandler : IRequestHandler<CreateConnectionCommand, bool>
    {
        private readonly IConnectionsRepository _connectionRepository;

        public CreateConnectionHandler(IConnectionsRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public async Task<bool> Handle(CreateConnectionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Guid sourceId = Guid.Parse(request.Request.SourceId);
                Guid targetId = Guid.Parse(request.Request.TargetId);
                await _connectionRepository.CreateConnectionAsync(sourceId, targetId, request.Request.ConnectionName);
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
