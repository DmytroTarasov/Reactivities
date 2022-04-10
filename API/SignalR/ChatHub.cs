using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command) {
            var comment = await _mediator.Send(command); // comment is a Result<CommandDto> object 

            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value); // comment.Value - Value is a property inside a Result class
        }

        // we override a method from a Hub class so that the user joins a group automatically when 
        // he connects to an application
        public override async Task OnConnectedAsync() {
            // we need to somehow get an access to an activityId (to be able to add that user to the appropriate group)
            var httpContext = Context.GetHttpContext();

            var activityId = httpContext.Request.Query["activityId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            // get the list of comments associated with the current activity
            var result = await _mediator.Send(new List.Query{ActivityId = Guid.Parse(activityId)});
            
            // send this comments to a client that is joining a group
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}