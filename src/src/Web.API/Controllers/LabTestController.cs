
using Application.Business.LabTests.Commands.CreateLabTest;
using Application.Business.LabTests.Commands.DeleteLabTest;
using Application.Business.LabTests.Commands.UpdateLabTest;
using Application.Business.LabTests.Queries;
using Application.Common.Security;
using Domain.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "administrator, supervisor")]
    public class LabTestController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public LabTestController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IEnumerable<LabTestDetail>> GetTests()
        {
            return await Mediator.Send(new GetLabTest());
        }
        [HttpGet("TestCategory")]
        public async Task<IEnumerable<LabCategoryDetail2>> GetLabTestCategories()
        {
            return await Mediator.Send(new GetLabCategories());
        }
        [HttpGet("LabTestDropDown")]
        public async Task<IEnumerable<LabTestPriceDetail2>> GetLabTestDropDown()
        {
            return await Mediator.Send(new GetLabTestDropDown());
        }
        [HttpGet("{id}")]
        public async Task<LabTestDetailById>  GetLabTestById(string id)
        {
            var query = new GetLabTestDetailById()
            {
                LabTestId = id
            };
            return await Mediator.Send(query);

        }
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateLabTestCommand command)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            command.MobilePhone = user.PhoneNumber;
            return await Mediator.Send(command);

        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateLabTestCommand command)
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
            var command = new DeleteLabTestCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
