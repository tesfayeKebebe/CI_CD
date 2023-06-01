using Application.Business.SelectedTestDetails.Queries;
using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications;

public class DraftHub: Hub<IDraftHub>
{
    public async Task SendMessage(SelectedTestDetail selectedTestDetail)
    {
        await Clients.All.BroadCastDraftData(selectedTestDetail);
    }
}