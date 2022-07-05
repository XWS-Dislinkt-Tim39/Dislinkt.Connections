﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.ApproveFollow.Commands;
using Dislinkt.Connections.Application.Block.Commands;
using Dislinkt.Connections.Application.CreateFollowRequest.Commands;
using Dislinkt.Connections.Application.Follow.Commands;
using Dislinkt.Connections.Application.GetFollowingPrivate.Commands;
using Dislinkt.Connections.Application.GetFollowRequests.Commands;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Application.RemoveConnection.Commands;
using Dislinkt.Connections.Application.Unfollow.Commands;
using Dislinkt.Connections.Persistence.Neo4j;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Constructor.
        /// </summary>
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
        [Authorize]
        [Route("/getFollowing")]
        public async Task<IReadOnlyList<Guid>> GetFollowingAsync(Guid sourceId)
        {
            return await _mediator.Send(new GetFollowingCommand(sourceId));
        }

        /// <summary>
        /// Follows a user.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/follow")]
        public async Task<bool> FollowAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new FollowCommand(connectionData));
        }

        /// <summary>
        /// Unfollows a user.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/unfollow")]
        public async Task<bool> UnfollowAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new UnfollowCommand(connectionData));
        }

        /// <summary>
        /// Creates a follow request for private profiles.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/createFollowRequest")]
        public async Task<bool> CreateFollowRequestAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new CreateFollowRequestCommand(connectionData));
        }


        /// <summary>
        /// Gets all follow requests for given ID.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("/getFollowRequests")]
        public async Task<IReadOnlyList<Guid>> GetFollowRequestsAsync(Guid sourceId)
        {
            return await _mediator.Send(new GetFollowRequestsCommand(sourceId));
        }

        /// <summary>
        /// Approves a follow request.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/approveFollow")]
        public async Task<bool> ApproveFollowAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new ApproveFollowCommand(connectionData));
        }

        /// <summary>
        /// Blocks a user and automatically removes all relationships between them.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/block")]
        public async Task<bool> BlockAsync(ConnectionData connectionData)
        {
            return await _mediator.Send(new BlockCommand(connectionData));
        }

        /// <summary>
        /// Gets all blocked users for a given ID.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("/getBlocked")]
        public async Task<bool> GetBlockedAsync(Guid sourceId)
        {
            return false;
        }

    }
}
