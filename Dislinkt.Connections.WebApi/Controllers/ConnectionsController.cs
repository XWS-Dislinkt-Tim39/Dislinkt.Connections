using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.CreateConnection.Commands;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Application.RemoveConnection.Commands;
using Dislinkt.Connections.Persistence.Neo4j;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dislinkt.Connections.WebApi.Controllers
{

    public class Test
    {
        public int TestAttr { get; set; }
        public int TestAttr2 { get; set; }

        public Test()
        {
            TestAttr = 1;
            TestAttr2 = 2;
        }
    }
    /// <summary>
    /// Main controller
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : Controller
    {
        private readonly IMediator _mediator;

        public ConnectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// INdex
        /// </summary>
        [HttpGet]
        [Route("/")]
        public string Index()
        {
            return "hello";
        }

        [HttpGet]
        [Route("/db")]
        public async Task<string> TestDb()
        {
            return await Neo4jExe.Example();
        }

        [HttpPost]
        [Route("/register-user")]
        public async Task<bool> RegisterUserAsync(UserData userData)
        {
            return await _mediator.Send(new RegisterUserCommand(userData));
        }

        [HttpGet]
        [Route("/testing")]
        public async Task Testing()
        {
            
        }

        [HttpPost]
        [Route("/createConnection")]
        public async Task<bool> CreateConnection(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateConnectionCommand(connectionData));
        }

        [HttpPost]
        [Route("/removeConnection")]
        public async Task<bool> RemoveConnection(ConnectionData connectionData)
        {
            return await _mediator.Send(new RemoveConnectionCommand(connectionData));
        }
    }
}
