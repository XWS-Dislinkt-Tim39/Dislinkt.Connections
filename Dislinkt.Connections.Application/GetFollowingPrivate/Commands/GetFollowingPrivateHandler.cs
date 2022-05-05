using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using MediatR;

namespace Dislinkt.Connections.Application.GetFollowingPrivate.Commands
{
    public class GetFollowingPrivateHandler : IRequestHandler<GetFollowingPrivateCommand, IReadOnlyList<Guid>>
    {
        private readonly IConnectionsRepository _connectionRepository;

        public GetFollowingPrivateHandler(IConnectionsRepository connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }

        public async Task<IReadOnlyList<Guid>> Handle(GetFollowingPrivateCommand request,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<Guid> retVal = new List<Guid>();
            try
            {
                retVal = await _connectionRepository.GetFollowingPrivate(request.SourceId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }

            return retVal;
        }
    }
}
