using Application.Interfaces;
using Domain.Entities;
using Domain.Events.BankAccounts;
using MediatR;

namespace Application.Business.BankAccounts.Commands.CreateBankAccount
{
    public record CreateBankAccountCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
        public long Account { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class CreateBankAccountCommandHandler : IRequestHandler<CreateBankAccountCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateBankAccountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = new BankAccount
            {
                Account = request.Account,
                Id = Guid.NewGuid().ToString(),
                CreatedBy = request.CreatedBy,
                Name = request.Name,
            };
            entity.AddDomainEvent(new BankAccountCreatedEvent(entity));
            _context.BankAccount.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
    }
}
