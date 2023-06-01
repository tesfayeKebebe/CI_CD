using Application.Interfaces;
using Domain.Enums;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestDetails.Queries;

public record GetSelectedTestDetailByUser: IRequest<IEnumerable<SelectedTestDetail>>
    {
    public required string UserId { get; set; }
    }

public class GetSelectedTestDetailByUserQueries : IRequestHandler<GetSelectedTestDetailByUser, IEnumerable<SelectedTestDetail>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetSelectedTestDetailByUserQueries(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<IEnumerable<SelectedTestDetail>> Handle(GetSelectedTestDetailByUser request,
        CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            var testResultStatus = await _context.TestResult.Where(x => x.IsCompleted).ToListAsync(cancellationToken);
                                                       return (from status in await _context.SelectedTestStatus.Where(x => x.TestStatus == TestStatus.OnProgress && x.AssignedUser==request.UserId)
                    .OrderBy(x => x.Created).ToListAsync(cancellationToken)
                let user = users.FirstOrDefault(x => x.Id == status.CreatedBy)
                where user != null
                select new SelectedTestDetail
                {
                    Amount = status.Amount,
                    Status = Enum.GetName(typeof(TestStatus), status.TestStatus),
                    TransactionNumber = status.TransactionNumber,
                    PatientName = user.FullName,
                    PatientId = user.Id,
                    IdNumber = user.IdNumber,
                    PhoneNumber = user.PhoneNumber,
                    DoneBy = users.FirstOrDefault(x => x.Id == status.LastModifiedBy)?.FullName,
                    Date = status.TestStatus == TestStatus.Completed ? status.LastModified.Value : status.Created,
                    Longitude = Convert.ToDouble(status.Longitude),
                    Latitude = Convert.ToDouble(status.Latitude),
                    AssignedUser = users.FirstOrDefault(x => x.Id == status.AssignedUser)?.FullName,
                    AssignUserId = status.AssignedUser,
                    IsCompleted = Convert.ToBoolean(testResultStatus
                        .FirstOrDefault(x => x.TransactionNumber == status.TransactionNumber)?.IsCompleted),
                    IsSampleTaken = status.IsSampleTaken,
                    SampleNumber = status.SampleNumber
                }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}