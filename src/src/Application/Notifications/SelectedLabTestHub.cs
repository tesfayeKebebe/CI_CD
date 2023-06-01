using Application.Business.SelectedTestDetails.Queries;
using Microsoft.AspNetCore.SignalR;

namespace Application.Notifications;

public class SelectedLabTestHub : Hub<ISelectedLabTestHub>
{
    public async Task SendMessage(SelectedTestDetail selectedTestDetail)
    {
        await Clients.All.BroadCastData(selectedTestDetail);
    }
}