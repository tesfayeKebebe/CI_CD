using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

public class LabCreatedEventHandler : INotificationHandler<LabCreatedEvent>
{
    private readonly ILogger<LabCreatedEventHandler> _logger;

    public LabCreatedEventHandler(ILogger<LabCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LabCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
