using Microsoft.AspNetCore.Mvc;
using Web.API.Helpers;

namespace Web.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmailController : ApiControllerBase
{
    private readonly IEmailService _emailService;
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<ActionResult>  SendEmail([FromBody] EmailDto email)
    {
        _emailService.SendEmail(email);
        return NoContent();
    }
}