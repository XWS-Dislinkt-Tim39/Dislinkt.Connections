using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dislinkt.Connections.Core.Repositories;
using Dislinkt.Connections.Domain.Users;
using MediatR;

namespace Dislinkt.Connections.Application.RegisterUser.Commands
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IConnectionsRepository _connectionsRepository;

        public RegisterUserHandler(IConnectionsRepository connectionsRepository)
        {
            _connectionsRepository = connectionsRepository;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _connectionsRepository.CreateUser(new User(Guid.Parse(request.Request.Id),
                    request.Request.UserName,
                    (VisibilityStatus)request.Request.Status));
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
