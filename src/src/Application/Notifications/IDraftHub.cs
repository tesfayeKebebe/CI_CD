using Application.Business.SelectedTestDetails.Queries;

namespace Application.Notifications;

public interface IDraftHub
{
    Task BroadCastDraftData(SelectedTestDetail selectedTestDetail);
}