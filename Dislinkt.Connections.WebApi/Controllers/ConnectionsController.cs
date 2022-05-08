using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.ApproveFollow.Commands;
using Dislinkt.Connections.Application.CreateConnection.Commands;
using Dislinkt.Connections.Application.CreateFollowRequest.Commands;
using Dislinkt.Connections.Application.Follow.Commands;
using Dislinkt.Connections.Application.GetFollowingPrivate.Commands;
using Dislinkt.Connections.Application.GetFollowRequests.Commands;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Application.RemoveConnection.Commands;
using Dislinkt.Connections.Persistence.Neo4j;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dislinkt.Connections.WebApi.Controllers
{

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
        [Route("/registerUser")]
        public async Task<bool> RegisterUserAsync(UserData userData)
        {
            return await _mediator.Send(new RegisterUserCommand(userData));
        }

        /// <summary>
        /// Creates a relationship between 2 users. (e.g. a-[:FOLLOWS]->b)
        /// </summary>
        [HttpPost]
        [Route("/createConnection")]
        public async Task<bool> CreateConnectionAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateConnectionCommand(connectionData));
        }

        /// <summary>
        /// Removes a relationship between 2 users.
        /// </summary>
        [HttpPost]
        [Route("/removeConnection")]
        public async Task<bool> RemoveConnectionAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new RemoveConnectionCommand(connectionData));
        }

        /// <summary>
        /// Given a UserID, returns the list of followed private users.
        /// </summary>
        [HttpGet]
        [Route("/getFollowing")]
        public async Task<IReadOnlyList<Guid>> GetFollowingAsync(Guid sourceId)
        {
            return await _mediator.Send(new GetFollowingCommand(sourceId));
        }

        /// <summary>
        /// Follows a user.
        /// </summary>
        [HttpPost]
        [Route("/follow")]
        public async Task<bool> FollowAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new FollowCommand(connectionData));
        }

        /// <summary>
        /// Creates a follow request for private profiles.
        /// </summary>
        [HttpPost]
        [Route("/createFollowRequest")]
        public async Task<bool> CreateFollowRequestAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateFollowRequestCommand(connectionData));
        }

        [HttpGet]
        [Route("/getFollowRequests")]
        public async Task<IReadOnlyList<Guid>> GetFollowRequestsAsync(Guid sourceId)
        {
            return await _mediator.Send(new GetFollowRequestsCommand(sourceId));
        }

        /// <summary>
        /// Approves a follow request.
        /// </summary>
        [HttpPost]
        [Route("/approveFollow")]
        public async Task<bool> ApproveFollowAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new ApproveFollowCommand(connectionData));
        }



    }
}
