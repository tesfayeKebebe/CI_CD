using Application.Business.LabTests.Queries;
using Application.Business.SampleTypes.Commands.CreateSampleType;
using Application.Business.SampleTypes.Commands.UpdateLab;
using Application.Business.SampleTypes.Queries;
using Application.Business.TubeTypes.Commands.DeleteTubeType;
using Application.Business.TubeTypes.Commands.UpdateTube;
using Application.Common.Security;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class SampleTypeController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<SampleTypeDetail>> GetSamples()
        {
            return await Mediator.Send(new GetSampleTypes());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateSampleTypeCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateSampleTypeCommand? command)
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
 

