using Domain.Events.SelectedTestStatuses;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Application.Business.SelectedTestStatuses.EventHandler;
public class SelectedTestStatusCreatedEventHandler : INotificationHandler<SelectedTestStatusCreatedEvent>
{
    private readonly ILogger<SelectedTestStatusCreatedEventHandler> _logger;

    public SelectedTestStatusCreatedEventHandler(ILogger<SelectedTestStatusCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SelectedTestStatusCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}