using Application.Interfaces;
using Domain.Enums;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestDetails.Queries;

public record GetSelectedTestStatusById: IRequest<SelectedTestDetail>
    {
        public required  string Id { get; set; }
    }
    public class GetSelectedTestStatusByIdQueries : IRequestHandler<GetSelectedTestStatusById, SelectedTestDetail>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetSelectedTestStatusByIdQueries(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

    
        public async Task<SelectedTestDetail> Handle(GetSelectedTestStatusById request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userManager.Users.ToListAsync(cancellationToken);

                var status = await _context.SelectedTestStatus.FirstOrDefaultAsync(x =>
                    x.TestStatus == TestStatus.OnProgress && x.Id == request.Id, cancellationToken);
                var user = users.FirstOrDefault(x => x.Id == status?.CreatedBy);
                if (user == null && status==null)
                {
                    return new SelectedTestDetail();
                }

                var data= new SelectedTestDetail
                {
                    Amount = status.Amount,
                    Status = Enum.GetName(typeof(TestStatus), status.TestStatus),
                    TransactionNumber = status.TransactionNumber,
                    PatientName = user.FullName,
                    PatientId = user.Id,
                    IdNumber = user.IdNumber,
                    PhoneNumber = user.PhoneNumber,
                    DoneBy = users.FirstOrDefault(x => x.Id == status.LastModifiedBy)?.FullName,
                    Date =  status.Created,
                    Longitude = Convert.ToDouble(status.Longitude),
                    Latitude = Convert.ToDouble(status.Latitude),
                    AssignedUser = users.FirstOrDefault(x => x.Id == status.AssignedUser)?.FullName,
                    AssignUserId = status.AssignedUser,
                    IsSampleTaken = status.IsSampleTaken,
                    SampleNumber = status.SampleNumber
                };
                return data;
            }
            catch (Exception e)
            {
            throw;
        }
           
        }
    }
