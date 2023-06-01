using Application.Business.PatientFiles.Commands.CreatePatientFile;
using Application.Business.PatientFiles.Commands.DeleteCreatePatientFile;
using Application.Business.PatientFiles.Commands.UpdateUserBranch;
using Application.Business.PatientFiles.Queries;
using Application.Business.UserBranches.Queries;
using Application.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Web.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PatientFileController : ApiControllerBase
{
    private readonly IHubContext<PatientFileHub,IPatientFileHub> _hubContext;
    public PatientFileController(IHubContext<PatientFileHub, IPatientFileHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<IEnumerable<PatientFileDetail>>  GetPatientFiles( )
    {
        return await Mediator.Send(new GetPatientFile());
    }
    [HttpGet("{patientId}")]
    public async Task<IEnumerable<PatientFileDetail>>  GetPatientFilesByPatientId( string patientId)
    {
        return await Mediator.Send(new GetPatientFilesByPatientId{PatientId = patientId});
    }
    [HttpPost]
    public async Task<ActionResult<string>>  Create([FromBody] List<CreatePatientFileCommand>  commands)
    {
        try
        {
            foreach (var command in commands)
            {
                await Mediator.Send(command);
            }
            var data = await Mediator.Send(new GetPatientFilesByPatientId {PatientId = commands.FirstOrDefault()?.CreatedBy}) ;
            await _hubContext.Clients.All.BroadCastPatientData(data);
            return "Created Successfully";
        }
        catch (Exception e)
        {
            throw new Exception("Fail to create file");
        }

    }
    [HttpPut]
    public async Task<ActionResult>  Update([FromBody] UpdatePatientFileCommand command)
    {
        if(command.Id == null)
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
        var command = new DeletePatientFileByIdCommand
        {
            Id = id
        };
        await Mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("patient-file/{id}")]
    public async Task<ActionResult> DeleteByPatientId(string id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var command = new DeletePatientFileCommand
        {
            PatientId = id
        };
        await Mediator.Send(command);
        return NoContent();
    }

}