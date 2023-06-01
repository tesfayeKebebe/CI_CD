using Application.Business.Organizations.Commands.CreateOrganization;
using Application.Business.Organizations.Commands.UpdateOrganization;
using Application.Business.Organizations.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ApiControllerBase
    {
        
        [HttpGet]
        public async Task<OrganizationDetail>  GetOrganization( )
        {
            return await Mediator.Send(new GetOrganization());
        }
        [Authorize (Roles ="administrator, supervisor" )]
        [HttpPost]
        public async Task<ActionResult<string>>  Create([FromBody] CreateOrganizationCommand command)
        {
         return  await Mediator.Send(command);
        }
        [Authorize (Roles ="administrator, supervisor" )]
        [HttpPut]
        public async Task<ActionResult>  Update([FromBody] UpdateOrganizationCommand command)
        {
            if(command.Id == null)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
        

    }
}
