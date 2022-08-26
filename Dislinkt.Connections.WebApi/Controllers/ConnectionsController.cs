using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Dislinkt.Connections.Application.ApproveFollow.Commands;
using Dislinkt.Connections.Application.Block.Commands;
using Dislinkt.Connections.Application.CreateFollowRequest.Commands;
using Dislinkt.Connections.Application.Deleteuser.Commands;
using Dislinkt.Connections.Application.Follow.Commands;
using Dislinkt.Connections.Application.GetBlocked.Commands;
using Dislinkt.Connections.Application.GetFollowingPrivate.Commands;
using Dislinkt.Connections.Application.GetFollowRecommendations.Commands;
using Dislinkt.Connections.Application.GetFollowRequests.Commands;
using Dislinkt.Connections.Application.GetWhoBlocksMe.Commands;
using Dislinkt.Connections.Application.RegisterUser.Commands;
using Dislinkt.Connections.Application.RejectFollow.Commands;
using Dislinkt.Connections.Application.RemoveConnection.Commands;
using Dislinkt.Connections.Application.Unblock.Commands;
using Dislinkt.Connections.Application.Unfollow.Commands;
using Dislinkt.Connections.Persistence.Neo4j;
using Grpc.Net.Client;
using GrpcAddActivityService;
using GrpcAddNotificationService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

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
        private readonly ITracer _tracer;
        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectionsController(IMediator mediator, ITracer tracer)
        {
            _mediator = mediator;
            _tracer = tracer;
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
        /// Register user,create node
        /// </summary>
        [HttpPost]
        [Route("/registerUser")]
        public async Task<bool> RegisterUserAsync(UserData userData)
        {
            return await _mediator.Send(new RegisterUserCommand(userData));
        }
        /// <summary>
        /// Register user,create node
        /// </summary>
        [HttpDelete]
        [Route("/deleteUser/{id}")]
        public async Task<bool> DeleteUser(Guid id)
        {
            return await _mediator.Send(new DeleteUserCommand(id));
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var channel = GrpcChannel.ForAddress("https://localhost:5003/");
            var client = new addActivityGreeter.addActivityGreeterClient(channel);
            var reply= client.addActivity(new ActivityRequest { UserId = connectionData.SourceId, Text ="Create connection", Type = "Connection", Date = DateTime.Now.ToString() });

            if (!reply.Successful)
            {
                Debug.WriteLine("Doslo je do greske prilikom kreiranja notifikacija za usera");
                return false;
            }

            Debug.WriteLine("Uspesno prosledjen na registraciju u notifikacijama -- " + reply.Message);

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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            await _mediator.Send(new CreateFollowRequestCommand(connectionData));

            var channel = GrpcChannel.ForAddress("https://localhost:5002/");
            var client = new addNotificationGreeter.addNotificationGreeterClient(channel);
            var reply = client.addNotification(new NotificationRequest { UserId = connectionData.SourceId, From = connectionData.TargetId, Type = "FriendRequest", Seen = false });

                if (!reply.Successful)
                {
                    Debug.WriteLine("Doslo je do greske prilikom kreiranja notifikacija za usera");
                    return false;
                }

                Debug.WriteLine("Uspesno prosledjen na registraciju u notifikacijama -- " + reply.Message);

           
            return true;

        }


        /// <summary>
        /// Gets all follow requests for given ID.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("/getFollowRequests")]
        public async Task<IReadOnlyList<Guid>> GetFollowRequestsAsync(Guid sourceId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new ApproveFollowCommand(connectionData));
        }


        /// <summary>
        /// Reject a follow request.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/rejectFollow")]
        public async Task<bool> RejectFollowAsync(ConnectionData connectionData)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new RejectFollowCommand(connectionData));
        }

        /// <summary>
        /// Blocks a user and automatically removes all relationships between them.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/block")]
        public async Task<bool> BlockAsync(ConnectionData connectionData)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new BlockCommand(connectionData));
        }

        /// <summary>
        /// Unblocks a user. Does not restore previous relationships between them.
        /// </summary>
        [HttpPost]
        [Authorize]
        [Route("/unblock")]
        public async Task<bool> UnblockBlockAsync(ConnectionData connectionData)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new UnblockCommand(connectionData));
        }

        /// <summary>
        /// Gets all blocked users for a given ID.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("/getBlocked")]
        public async Task<IReadOnlyList<Guid>> GetBlockedAsync(Guid sourceId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new GetBlockedCommand(sourceId));
        }

        /// <summary>
        /// Gets the users that block the user with given ID.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("/getWhoBlocksMe")]
        public async Task<IReadOnlyList<Guid>> GetWhoBlocksMe(Guid sourceId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new GetWhoBlocksMeCommand(sourceId));
        }

        [HttpGet]
        [Authorize]
        [Route("/getFollowRecommendations")]
        public async Task<IReadOnlyList<Guid>> GetFollowRecommendations(Guid sourceId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _mediator.Send(new GetFollowRecommendationsCommand(sourceId));
        }


    }
}
