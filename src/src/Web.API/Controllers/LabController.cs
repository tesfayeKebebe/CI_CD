
using Application.Business.Labs.Commands.CreateLab;
using Application.Business.Labs.Commands.DeleteLab;
using Application.Business.Labs.Commands.UpdateLab;
using Application.Business.Labs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class LabController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<LabDetail>>  GetLabs( )
        {
            return await Mediator.Send(new GetLab());
        }
        [HttpPost]
        public async Task<ActionResult<string>>  Create([FromBody] CreateLabCommand command)
        {
         return  await Mediator.Send(command);
        }
        [HttpPut]
        public async Task<ActionResult>  Update([FromBody] UpdateLabCommand command)
        {
            if(command.Id == null)
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
            var command = new DeleteLabCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
