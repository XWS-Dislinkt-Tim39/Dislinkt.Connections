using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Dislinkt.Connections.Application.Common.Commands
{
    public interface ICommand<out T> : IRequest
    {
        T Request { get; }
    }
}
