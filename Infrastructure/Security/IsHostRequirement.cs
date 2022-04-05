using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
        
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Task.CompletedTask; // user doesn`t satisfy to the authorization requirements (is not a 
            // host of the activity so he cannot edit it)

            // when the request comes, it has an activity id as it`s route values - so here we try to get this activity id
            // and then parse it to the 'Guid' type
            var activityId = Guid.Parse(
                _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id")
                .Value?.ToString());
            
            // here, we use .Result because we cannot use 'await' inside this method 
            // (we implement an abstract class 'AuthorizationHandler' and this method returns a Task (not async Task))
            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId).Result;

            if (attendee == null) return Task.CompletedTask;

            if (attendee.IsHost) context.Succeed(requirement); // user is allowed to edit an activity

            return Task.CompletedTask;
        }
    }
}