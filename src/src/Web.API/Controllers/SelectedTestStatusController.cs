using Application.Business.SelectedTestDetails.Commands.UpdateSelectedTestDetail;
using Application.Business.SelectedTestDetails.Queries;
using Application.Business.SelectedTestStatuses.Commands.UpdateSelectedTestStatus;
using Application.Common.Security;
using Application.Interfaces;
using Application.Notifications;
using Domain.Enums;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.API.Helpers;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SelectedTestStatusController : ApiControllerBase
    {
        private readonly IHubContext<SelectedLabTestHub,ISelectedLabTestHub> _hubContext;
        private readonly IHubContext<DraftHub,IDraftHub> _draftHubContext;
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        public SelectedTestStatusController(IHubContext<SelectedLabTestHub, ISelectedLabTestHub> hubContext, IApplicationDbContext context, IEmailService emailService, UserManager<ApplicationUser> userManager, IHubContext<DraftHub, IDraftHub> draftHubContext)
        {
            _hubContext = hubContext;
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
            _draftHubContext = draftHubContext;
        }

        [HttpPut]
        public async Task<ActionResult<string>>  Update([FromBody] UpdateSelectedTestStatusCommand command)
        {
            var data=  await Mediator.Send(command);
            var selectedDetailCommand = new GetSelectedTestStatusById
            {
                Id = data
            };
            var selectedDetail = await Mediator.Send(selectedDetailCommand);
            if (command.TestStatus == TestStatus.Completed)
            {
                var entity = await  _context.TestResult.FirstOrDefaultAsync(x=>x.TransactionNumber== command.TransactionNumber);

                if (entity is {Attachments: { }})
                {
                    var user = await _userManager.FindByIdAsync(entity.PatientId);
                    var to = user?.Email;
                    if (to != "user@gmail.com")
                    {
                        var emailDto = new EmailDto
                        {
                            Body = entity.Description,
                            Attachments = entity.Attachments,
                            Subject = "Your Laboratory Result",
                            To = to,
                            FileName = entity.FileName,
                            ContentType = entity.ContentType,
                            StoredFileName = entity.StoredFileName
                        };
                        await _emailService.SendEmail(emailDto);       
                    }

                }
            }

            if (command.TestStatus == TestStatus.Draft)
            {
                await _draftHubContext.Clients.All.BroadCastDraftData(selectedDetail);
            }
            if (command.TestStatus == TestStatus.OnProgress)
            {
                await _hubContext.Clients.All.BroadCastData(selectedDetail);  
            }
            return NoContent();
        }
        [HttpPut("AssignUser")]
        public async Task<ActionResult<string>>  Update([FromBody] UpdateAssignedUserCommand command)
        {
          var data=  await Mediator.Send(command);
            var selectedDetailCommand = new GetSelectedTestStatusById
            {
                Id = data
            };
            var selectedDetail = await Mediator.Send(selectedDetailCommand);
            await _hubContext.Clients.All.BroadCastData(selectedDetail);
            return NoContent();
        }
        [HttpPut("Sample")]
        public async Task<ActionResult<string>>  UpdateIsSample([FromBody] UpdateSelectedTestSampleCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
 
        
    }
}