using Domain.Events.Categories;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Application.Business.Categories.EventHandler
{
    public class CategoryCreatedEventHandler : INotificationHandler<CategoryCreatedEvent>
    {
        private readonly ILogger<LabCreatedEventHandler> _logger;

        public CategoryCreatedEventHandler(ILogger<LabCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
