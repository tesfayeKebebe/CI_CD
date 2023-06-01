using System.Net;
using Application.Business.LabTestResults.Commands.CreateLabTestResult;
using Application.Business.LabTestResults.Commands.UpdateLabTestResult;
using Application.Business.LabTestResults.Queries;
using Application.Common.Security;
using Application.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.API.Helpers;

namespace Web.API.Controllers
{
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TestResultController : ApiControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<FileSaveController> logger;
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    public TestResultController(IWebHostEnvironment env, ILogger<FileSaveController> logger, IApplicationDbContext context, IEmailService emailService, UserManager<ApplicationUser> userManager)
    {
        this.env = env;
        this.logger = logger;
        _context = context;
        _emailService = emailService;
        _userManager = userManager;
    }

    [HttpGet("{transactionNumber}")]
    public async Task<TestResultDetail> Get(string transactionNumber)
    {
        var query = new GetLabTestResults
        {
            TransactionNumber = transactionNumber
        };
        return await Mediator.Send(query);
    }
    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateLabTestResultCommand command)
    {
        if (command.StoredFileName != null)
        {
            var physicalPath  = Path.Combine(env.ContentRootPath,  "wwwroot", "Files", command.StoredFileName);;
            var pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            command.Attachments = pdfBytes;
        }
        return await Mediator.Send(command);
    }
    [HttpPut]
    public async Task<ActionResult> Update( [FromBody] UpdateLabTestResultCommand? command)
    {
        if (command == null)
        {
            return BadRequest();
        }

        if (command.StoredFileName != null)
        {
            var physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "Files", command.StoredFileName);
            var pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            command.Attachments = pdfBytes;
        }
        await Mediator.Send(command);
        return NoContent();
    }
    [HttpPut("Approval")]
    public async Task<ActionResult> Update( [FromBody] LabTestResultApprovalCommand? command)
    {
        if (command == null)
        {
            return BadRequest();
        }
        await Mediator.Send(command);
        return NoContent();
    }

   
}
}
