using Application.Activities;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        // ActivityParams - we have created this class that actually holds an info about how to filter activities list
        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param) {
            // send a request to a single handler
            return HandlePagedResult(await Mediator.Send(new List.Query{Params = param}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id) {
            // HandleResult() is a method from a BaseApiController
            // (a base class for all our controllers)
            return HandleResult(await Mediator.Send(new Details.Query{ Id = id })); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody]Activity activity) {
            return HandleResult(await Mediator.Send(new Create.Command{ Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity) {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id) {
            return HandleResult(await Mediator.Send(new Delete.Command{ Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id) { // id - activity id
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command{ Id = id}));
        }
    }
}