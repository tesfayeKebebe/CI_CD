using Domain.Entities;
using Domain.Events.LabTests;
using MediatR;
using Application.Interfaces;
namespace Application.Business.LabTests.Commands.CreateLabTest
{
    public record CreateLabTestCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public bool IsFastingRequired { get; set; }
        public string? Description { get; set; }
        public IList<string> TubeTypeId { get; set; } = new List<string>();
        public  IList<string> SampleTypeId { get; set; }=new List<string>();
        public string? MobilePhone { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class CreateLabTestCommandHandler : IRequestHandler<CreateLabTestCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMessagingService _messagingService;
        public CreateLabTestCommandHandler(IApplicationDbContext context, IMessagingService messagingService)
        {
            _context = context;
            _messagingService = messagingService;

        }
        public async Task<string> Handle(CreateLabTestCommand request, CancellationToken cancellationToken)
        {
            var entity = new LabTest
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Id = Guid.NewGuid().ToString(), 
                Description = request.Description,
                IsFastingRequired = request.IsFastingRequired,
                CreatedBy = request.CreatedBy
            };
            entity.AddDomainEvent(new LabTestCreatedEvent(entity));
            _context.LabTest.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var sample in request.SampleTypeId)
            {
                var samp = new LabTestSampleTypeDetail
                {
                     LabTestId = entity.Id,
                    SampleTypeId = sample,
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = request.CreatedBy
                };
                _context.LabTestSampleTypeDetail.Add(samp);
                await _context.SaveChangesAsync(cancellationToken);
            }
            foreach (var tube in request.TubeTypeId)
            {
                var tub = new LabTestTubeTypeDetail
                {
                    LabTestId = entity.Id,
                    TubeTypeId = tube,
                    Id = Guid.NewGuid().ToString(), 
                    CreatedBy = request.CreatedBy
                };
                _context.LabTestTubeTypeDetail.Add(tub);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // if (request?.MobilePhone != null)
            // {
            //     var result = await _messagingService.SendMessage(request.MobilePhone);
            // }

            // general_obj.WriteLog(mobile_number +" "+message, "SendLog");
            return entity.Id;

        }
}
}
