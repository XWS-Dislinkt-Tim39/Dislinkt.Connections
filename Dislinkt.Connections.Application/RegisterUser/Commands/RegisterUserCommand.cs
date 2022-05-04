using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.RegisterUser.Commands
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public RegisterUserCommand(UserData userData)
        {
            Request = userData;
        }
        public UserData Request { get; set; }
    }
}
