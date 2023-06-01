using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
public class LabCompletedEventHandler : INotificationHandler<LabCompletedEvent>
{
    private readonly ILogger<LabCompletedEventHandler> _logger;

    public LabCompletedEventHandler(ILogger<LabCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LabCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Service Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
