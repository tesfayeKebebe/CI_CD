
using Application.Business.ServiceCharges.Commands.CreateServiceCharge;
using Application.Business.ServiceCharges.Commands.DeleteServiceCharge;
using Application.Business.ServiceCharges.Commands.UpdateServiceCharge;
using Application.Business.ServiceCharges.Queries;
using Application.Common.Security;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class ServiceChargeController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ServiceChargeDetail>> GetServiceCharges()
        {
            return await Mediator.Send(new GetServiceCharge());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateServiceChargeCommand command)
        {
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateServiceChargeCommand command)
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
            var command = new DeleteServiceChargeCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
