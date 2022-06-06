using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dislinkt.Connections;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Domain.Users;
using MediatR;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IMediator _mediator;
        public GreeterService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            bool result;
            try
            {
                var userData = new UserData{ Id = request.Id, UserName = request.Username, Status = request.Status };
                result = await _mediator.Send(new RegisterUserCommand(userData));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return await Task.FromResult(new HelloReply
                {
                    Successful = false,
                    Message = $"{ex}"
                });
            }

            return await Task.FromResult(new HelloReply
            {
                Successful = result,
                Message = $"Korisnik: {request.Id} | {request.Username} | {request.Status}"
            });
        }
    }
}
