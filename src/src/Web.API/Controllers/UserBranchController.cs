using Application.Business.Labs.Commands.DeleteLab;
using Application.Business.UserAssigns.Commands.CreateUserAssign;
using Application.Business.UserAssigns.Commands.DeleteUserAssign;
using Application.Business.UserAssigns.Commands.UpdateUserAssign;
using Application.Business.UserAssigns.Queries;
using Application.Business.UserBranches.Commands.CreateUserBranch;
using Application.Business.UserBranches.Commands.UpdateUserBranch;
using Application.Business.UserBranches.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBranchController :  ApiControllerBase
    {
        [HttpGet]
    public async Task<IEnumerable<UserBranchDetail>> Get()
    {
        return await Mediator.Send(new GetUserBranch());
    }
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateUserBranchCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateUserBranchCommand command)
    {
        if (command.Id == null)
        {
            return BadRequest();
        }
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var command = new DeleteUserBranchCommand()
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
}
}
