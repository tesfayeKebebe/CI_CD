using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Business.BankAccounts.Commands.UpdateBankAccount
{
    public record UpdateBankAccountCommand    : IRequest
    {
        public string Name { get; set; } = null!;
        public long Account { get; set; }
        public string? Id { get; set; } = null!;
        public string? LastModifiedBy { get; set; }
    }
    public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBankAccountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await  _context.BankAccount.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(BankAccount), request.Id);
            }
            entity.Account = request.Account;
            entity.LastModifiedBy = request.LastModifiedBy;
            entity.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;

        }
    }
}
