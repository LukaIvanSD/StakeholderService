using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stakeholders.Api.Dtos;
using Stakeholders.Api.Dtos.Requests;
using Stakeholders.Api.Public;
using Stakeholders.Core.UseCases;

namespace Stakeholders.Controllers;

[ApiController]
[Route("api/users")]
[Authorize("administratorPolicy")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("paged")]
    public IActionResult GetPaged([FromQuery] PagedRequestDto request)
    {
        var result = _userService.GetPaged(request.Page, request.PageSize);
        return CreateResponse(result);
    }

    [HttpPatch("{id}/block-toggle")]
    public IActionResult ToggleBlock(long id)
    {
        var result = _userService.UpdateIsUserBlocked(id);
        return CreateResponse(result);
    }

}