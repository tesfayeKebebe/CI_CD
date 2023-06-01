using Application.Business.SelectedTestDetails.Queries;

namespace Application.Notifications;

public interface ISelectedLabTestHub
{
    Task BroadCastData(SelectedTestDetail selectedTestDetail);
}