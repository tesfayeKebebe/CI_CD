using Application.Business.TubeTypes.Commands.CreateTubeType;
using Application.Business.TubeTypes.Commands.DeleteTubeType;
using Application.Business.TubeTypes.Commands.UpdateTube;
using Application.Business.TubeTypes.Queries;
using Application.Common.Security;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class TubeTypeController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<TubeTypeDetail>> GetTubeTypes()
        {
            return await Mediator.Send(new GetTubeTypes());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateTubeTypeCommand command)
        {
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTubeTypeCommand? command)
        {
            if (command == null)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var command = new DeleteTubeTypeCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }   
}
