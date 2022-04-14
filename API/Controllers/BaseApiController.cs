using API.Extensions;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result) {
            if (result == null) return NotFound();
            if(result.IsSuccess && result.Value != null) 
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }

        // for pagination
        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result) {
            if (result == null) return NotFound();
            if(result.IsSuccess && result.Value != null) {
                // add a pagination header (AddPaginationHeader is just an extension method 
                // that we have created inside a HttpExtensions.cs file)
                Response.AddPaginationHeader(result.Value.CurrentPage, 
                    result.Value.PageSize, 
                    result.Value.TotalCount,
                    result.Value.TotalPages);
                return Ok(result.Value);
            }
                
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
    }
}