using Application.Business.Labs.Commands.CreateLab;
using Application.Business.Labs.Commands.DeleteLab;
using Application.Business.Labs.Commands.UpdateLab;
using Application.Business.Labs.Queries;
using Application.Business.UserAssigns.Commands.CreateUserAssign;
using Application.Business.UserAssigns.Commands.DeleteUserAssign;
using Application.Business.UserAssigns.Commands.UpdateUserAssign;
using Application.Business.UserAssigns.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssignController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<UserAssignDetail?>> Get()
        {
            return await Mediator.Send(new GetUserAssign());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateUserAssignCommand command)
        {
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateUserAssignCommand command)
        {
            if (command.Id == null)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpPut("UpdateByService")]
        public async Task<ActionResult> UpdateByService([FromBody] UpdateUserAssignCommandByService command)
        {
            if (command == null)
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
            var command = new DeleteUserAssignCommand()
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
    }
