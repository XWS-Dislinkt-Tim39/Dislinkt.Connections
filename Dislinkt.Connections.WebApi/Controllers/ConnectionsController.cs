using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.ApproveFollow.Commands;
using Dislinkt.Connections.Application.CreateConnection.Commands;
using Dislinkt.Connections.Application.CreateFollowRequest.Commands;
using Dislinkt.Connections.Application.GetFollowingPrivate.Commands;
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

        /// <summary>
        /// Creates a new node in graph database.
        /// </summary>

        [HttpPost]
        [Route("/register-user")]
        public async Task<bool> RegisterUserAsync(UserData userData)
        {
            return await _mediator.Send(new RegisterUserCommand(userData));
        }

        /// <summary>
        /// Creates a relationship between 2 users. (e.g. a-[:FOLLOWS]->b)
        /// </summary>
        [HttpPost]
        [Route("/createConnection")]
        public async Task<bool> CreateConnection(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateConnectionCommand(connectionData));
        }

        /// <summary>
        /// Removes a relationship between 2 users.
        /// </summary>
        [HttpPost]
        [Route("/removeConnection")]
        public async Task<bool> RemoveConnection(ConnectionData connectionData)
        {
            return await _mediator.Send(new RemoveConnectionCommand(connectionData));
        }

        /// <summary>
        /// Given a UserID, returns the list of followed private users.
        /// </summary>
        [HttpGet]
        [Route("/getFollowingPrivate")]
        public async Task<IReadOnlyList<Guid>> GetFollowingPrivate(Guid sourceId)
        {
            return await _mediator.Send(new GetFollowingPrivateCommand(sourceId));
        }

        [HttpPost]
        [Route("/createFollowRequest")]
        public async Task<bool> CreateFollowRequest(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateFollowRequestCommand(connectionData));
        }

        /// <summary>
        /// Approves a follow request.
        /// </summary>
        [HttpPost]
        [Route("/approveFollow")]
        public async Task<bool> ApproveFollow(ConnectionData connectionData)
        {
            return await _mediator.Send(new ApproveFollowCommand(connectionData));
        }
    }
}
