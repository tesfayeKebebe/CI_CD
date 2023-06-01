using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.SampleTypes.Commands.UpdateLab
{
    public record UpdateSampleTypeCommand    : IRequest
    {
        public string Name { get; set; } = null!;
        public string? Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? LastModifiedBy { get; set; }

    }
    public class UpdateSampleTypeCommandHandler : IRequestHandler<UpdateSampleTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSampleTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSampleTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.SampleType.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(SampleType), request.Id);
            }
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
            entity.LastModifiedBy = request.LastModifiedBy;
           await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
