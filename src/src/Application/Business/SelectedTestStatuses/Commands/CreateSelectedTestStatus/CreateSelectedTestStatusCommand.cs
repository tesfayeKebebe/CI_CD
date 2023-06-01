using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.SelectedTestStatuses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Business.SelectedTestStatuses.Commands.CreateSelectedTestStatus
{
    public record CreateSelectedTestStatusCommand : IRequest<string>
    {
        public string TransactionNumber { get; set; } = null!;
        public double TotalAmount { get; set; }
        public TestStatus TestStatus { get; set; }
        public string? CreatedBy { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }
    public class CreateSelectedTestStatusCommandHandler : IRequestHandler<CreateSelectedTestStatusCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateSelectedTestStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateSelectedTestStatusCommand request, CancellationToken cancellationToken)
        {
            var userAssigns =  (from user in await _context.UserAssign.ToListAsync(cancellationToken)
                select new UserDistance
                {
                    Distance = DistanceCalculatorService.DistanceTo(request.Latitude, request.Longitude,
                        Convert.ToDouble(user.Latitude), Convert.ToDouble(user.Longitude)),
                    UserId = user.UserId
                }).ToList().MinBy(x=>x.Distance);
            var testStatus = await _context.SelectedTestStatus.OrderByDescending(x => x.SampleNumber)
                .FirstOrDefaultAsync(cancellationToken);
            var entity = new SelectedTestStatus
            {
                TestStatus = request.TestStatus,
                TransactionNumber = request.TransactionNumber, 
                Amount = request.TotalAmount,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy,
                Latitude = 
                Convert.ToString(request.Latitude, CultureInfo.InvariantCulture) , 
                Longitude =Convert.ToString(request.Longitude, CultureInfo.InvariantCulture),
                AssignedUser = userAssigns?.UserId,
                SampleNumber = Convert.ToInt32(testStatus?.SampleNumber)+1
            };
            entity.AddDomainEvent(new SelectedTestStatusCreatedEvent(entity));
            _context.SelectedTestStatus.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}
