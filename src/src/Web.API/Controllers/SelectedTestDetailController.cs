using Application.Business.SelectedTestDetails.Commands.CreateSelectedTestDetail;
using Application.Business.SelectedTestDetails.Commands.DeleteSelectedTestDetail;
using Application.Business.SelectedTestDetails.Queries;
using Application.Business.SelectedTestStatuses.Commands.CreateSelectedTestStatus;
using Application.Common.Security;
using Application.Notifications;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SelectedTestDetailController : ApiControllerBase
    {
        private readonly IHubContext<SelectedLabTestHub,ISelectedLabTestHub> _hubContext;
        private readonly IHubContext<DraftHub,IDraftHub> _draftHubContext;
        public SelectedTestDetailController(IHubContext<SelectedLabTestHub,ISelectedLabTestHub> hubContext, IHubContext<DraftHub, IDraftHub> draftHubContext)
        {
            _hubContext = hubContext;
            _draftHubContext = draftHubContext;
        }

        [HttpGet("{status}")]
        public async Task<IEnumerable<SelectedTestDetail>>  GetSelectedTestDetails( TestStatus status )
        {
            var query = new GetSelectedTestDetails
            {
             TestStatus = status
            };
            var data = await Mediator.Send(query);
            return data;
        }
        [HttpGet("SelectedData/{userId}")]
        public async Task<IEnumerable<SelectedTestDetail>>  GetSelectedTestDetails( string userId )
        {
            var query = new GetSelectedTestDetailByUser()
            {
                UserId = userId
            };
            var data = await Mediator.Send(query);
            return data;
        }
        [HttpGet]
        public async Task<IEnumerable<SelectedTestDetail>>  GetSelectedTestDetailsByDate([FromQuery] GetSelectedTestDetailByDate query )
        {
            return await Mediator.Send(query);
        }
        [HttpGet("GetData/{parentId}")]
        public async Task<IEnumerable <SelectedLabTestDetailByParentId>> GetLabTestById(string parentId)
        {
            var query = new GetSelectedDetailByParentId()
            {
                ParentId = parentId
            };
            return await Mediator.Send(query);

        }
        [HttpPost("{status:int}/{latitude:double}/{longitude:double}")]
        public async Task<ActionResult<string>>  Create([FromBody] List<CreateSelectedTestDetailCommand> commands, int status,double latitude, double longitude)
        {
            double totalAmount = 0;
            foreach (var command in commands)
            {
                command.TransactionNumber = command.TransactionNumber;
                totalAmount += command.Price;
                await Mediator.Send(command);
            }
            
            var statusCommand = new CreateSelectedTestStatusCommand
            {
                TransactionNumber = commands[0].TransactionNumber, TotalAmount = totalAmount,
                TestStatus = (TestStatus)status
                ,Latitude = latitude ,
                Longitude = longitude,
                CreatedBy= commands[0].CreatedBy
            };
            var statusId = await Mediator.Send(statusCommand);
            var selectedDetailCommand = new GetSelectedTestStatusById
            {
                Id = statusId
            };
            var selectedDetail = await Mediator.Send(selectedDetailCommand);
            if ((TestStatus)status == TestStatus.Draft)
            {
                await _draftHubContext.Clients.All.BroadCastDraftData(selectedDetail);
            }
            if ((TestStatus)status == TestStatus.OnProgress)
            {
                await _hubContext.Clients.All.BroadCastData(selectedDetail);  
            }
            return statusId;
        }
 
        [HttpDelete("{transactionNumber}")]
        public async Task<ActionResult> Delete(string? transactionNumber)
        {
            if (transactionNumber == null)
            {
                return BadRequest();
            }
            var command = new DeleteSelectedTestDetailByTransactionCommand
            {
                TransactionNumber = transactionNumber
            };
           await Mediator.Send(command);
           return NoContent();
        }
    }
}
