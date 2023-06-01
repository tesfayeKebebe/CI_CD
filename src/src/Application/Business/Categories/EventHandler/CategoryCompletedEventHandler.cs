using Domain.Events.Categories;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Application.Business.Categories.EventHandler
{
    public class CategoryCompletedEventHandler : INotificationHandler<CategoryCompletedEvent>
    {
        private readonly ILogger<CategoryCompletedEventHandler> _logger;

        public CategoryCompletedEventHandler(ILogger<CategoryCompletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CategoryCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Health Service Domain Event: {DomainEvent}", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
