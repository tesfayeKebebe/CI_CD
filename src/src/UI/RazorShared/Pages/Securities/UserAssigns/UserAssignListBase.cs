using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Securities.UserAssigns.Create;
using RazorShared.Pages.Securities.UserAssigns.Edit;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.Security;
using RazorShared.Server.Models.UserAssigns;

namespace RazorShared.Pages.Securities.UserAssigns;

public class UserAssignListBase : ComponentBase
{
     [Inject] private IUserAssignService UserAssignService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<UserAssignDetail>? UserAssignDetails { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    private IList<UserAssignDetail>? OriginalData { get; set; } = null!;

    protected string? Search { get; set; }
    protected bool ClearEnabled { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }
    protected void OnInputChanged(ChangeEventArgs obj)
    {
        ClearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
        Search = obj.Value?.ToString();
        SearchAsync();
        StateHasChanged();
    }
    protected void Clear()
    {
        Search = null;
        ClearEnabled = false;
        if (OriginalData != null)
        {
            UserAssignDetails = OriginalData.ToList();
        }
    }

    private void SearchAsync()
    {
        if(Search==null) return;
        UserAssignDetails = new List<UserAssignDetail>();
        if (OriginalData == null)
        {
            return;
        }

        foreach (var testPrice in OriginalData
                     .Where(x => x.FullName != null && Search != null &&
                                 x.FullName.ToLower().Contains(Search.ToLower())).ToList().Select(user =>
                         new UserAssignDetail()
                         {
                             Id = user.Id,
                             Branch = user.Branch,
                             FullName = user.FullName,
                             UserName = user.UserName,
                             PhoneNumber = user.PhoneNumber,
                             IsActive = user.IsActive,
                             UserId = user.UserId,
                             UserBranchId = user.UserBranchId
                         }))
        {
            UserAssignDetails.Add(testPrice);
        }
    }
    private async Task GetData()
    {
        UserAssignDetails = await UserAssignService.GetUserAssign();
        OriginalData = UserAssignDetails;
    }
    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateUserAssign>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px"});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(UserAssignDetail user)
    {
        var userEdit = new UserAssignDto
        {
            Id = user.Id,
            IsActive = user.IsActive,
            UserId = user.UserId,
            UserBranchId = user.UserBranchId
        };
        await DialogService.OpenAsync<EditUserAssign>("",
            new Dictionary<string, object>() {{"Model", userEdit}},
            new DialogOptions {Width = "50%", Height = "570px"});
        await GetData();
        StateHasChanged();
    }
    protected async Task OnDelete(string id)
    {
        try
        {
            var confirm
                =  await DialogService.Confirm("Are you sure?", "Delete",
                    new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
            if (confirm.Equals(true))
            {
                var result = await UserAssignService.DeleteUserAssign(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
               await GetData();
               StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
    
}