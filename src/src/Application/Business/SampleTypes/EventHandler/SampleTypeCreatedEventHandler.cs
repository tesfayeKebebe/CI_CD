using Domain.Events.SampleTypes;
using MediatR;
using Microsoft.Extensions.Logging;
public class SampleTypeCreatedEventHandler : INotificationHandler<SampleTypeCreatedEvent>
{
    private readonly ILogger<SampleTypeCreatedEventHandler> _logger;

    public SampleTypeCreatedEventHandler(ILogger<SampleTypeCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SampleTypeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
