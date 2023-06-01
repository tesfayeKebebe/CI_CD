using Domain.Events.SelectedTestDetails;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Application.Business.SelectedTestDetails.EventHandler;
public class SelectedTestDetailCreatedEventHandler : INotificationHandler<SelectedTestDetailCreatedEvent>
{
    private readonly ILogger<SelectedTestDetailCreatedEventHandler> _logger;

    public SelectedTestDetailCreatedEventHandler(ILogger<SelectedTestDetailCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SelectedTestDetailCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}