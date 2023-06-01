using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.LabTestResults.Commands.UpdateLabTestResult;

public record LabTestResultApprovalCommand : IRequest
{
    public string Id { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string Reason { get; set; } = null !;
    public string? ApprovedBy { get; set; }
}
public class UpdateLabTestResultApprovalCommandHandler : IRequestHandler<LabTestResultApprovalCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLabTestResultApprovalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(LabTestResultApprovalCommand request, CancellationToken cancellationToken)
    {
        var entity = await  _context.TestResult.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(TestResult), request.Id);
        }
        entity.Reason = request.Reason;
        entity.IsCompleted = request.IsCompleted;
        entity.ApprovedBy = request.ApprovedBy;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}