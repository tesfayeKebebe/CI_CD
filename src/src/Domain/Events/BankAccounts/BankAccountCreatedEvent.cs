using Domain.Common;
using Domain.Entities;

namespace Domain.Events.BankAccounts
{
    public class BankAccountCreatedEvent : BaseEvent
    {
        public BankAccountCreatedEvent(BankAccount item)
        {
            BankAccount = item;
        }
        public BankAccount BankAccount { get; }
    }
}
