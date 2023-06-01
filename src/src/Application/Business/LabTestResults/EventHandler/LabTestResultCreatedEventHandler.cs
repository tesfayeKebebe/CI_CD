using Domain.Events.LabTestResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Business.LabTestResults.EventHandler;
public class LabTestResultsCreatedEventHandler : INotificationHandler<LabTestResultCreatedEvent>
{
    private readonly ILogger<LabTestResultsCreatedEventHandler> _logger;

    public LabTestResultsCreatedEventHandler(ILogger<LabTestResultsCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LabTestResultCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}