using Application.Business.TestPrices.Commands.CreateTestPrice;
using Application.Business.TestPrices.Commands.DeleteTestPrice;
using Application.Business.TestPrices.Commands.UpdateTestPrice;
using Application.Business.TestPrices.Queries;
using Application.Common.Security;
using Microsoft.AspNetCore.Mvc;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class TestPriceController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<TestPriceDetail>> GetTestPrices()
        {
            return await Mediator.Send(new GetTestPrice());
        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateTestPriceCommand command)
        {
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateTestPriceCommand command)
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
            var command = new DeleteTestPriceCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
