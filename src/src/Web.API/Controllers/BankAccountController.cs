
using Application.Business.BankAccounts.Commands.CreateBankAccount;
using Application.Business.BankAccounts.Commands.UpdateBankAccount;
using Application.Business.BankAccounts.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="administrator, supervisor" )]
    public class BankAccountController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<BankAccountDetail>>  GetBankAccount( )
        {
            return await Mediator.Send(new GetBankAccount());
        }
        [HttpPost]
        public async Task<ActionResult<string>>  Create([FromBody] CreateBankAccountCommand command)
        {
         return  await Mediator.Send(command);
        }
        [HttpPut]
        public async Task<ActionResult>  Update([FromBody] UpdateBankAccountCommand command)
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
