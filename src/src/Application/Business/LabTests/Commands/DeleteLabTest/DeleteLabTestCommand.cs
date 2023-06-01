using Application.Common.Exceptions;
using Domain.Entities;
using MediatR;
using Application.Interfaces;
namespace Application.Business.LabTests.Commands.DeleteLabTest
{
    public record DeleteLabTestCommand : IRequest
    {
        public string? Id { get; set; } = null!;
    }
    public class DeleteLabTestCommandHandler : IRequestHandler<DeleteLabTestCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteLabTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteLabTestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.LabTest.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(LabTest), request.Id);
            }
            entity.IsDeleted = true;
            entity.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
