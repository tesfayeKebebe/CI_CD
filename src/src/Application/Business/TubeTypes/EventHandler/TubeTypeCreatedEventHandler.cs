using Domain.Events.TubeTypes;
using MediatR;
using Microsoft.Extensions.Logging;
public class TubeTypeCreatedEventHandler : INotificationHandler<TubeTypeCreatedEvent>
{
    private readonly ILogger<TubeTypeCreatedEventHandler> _logger;

    public TubeTypeCreatedEventHandler(ILogger<TubeTypeCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TubeTypeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
