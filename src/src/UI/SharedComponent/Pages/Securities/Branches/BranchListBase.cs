using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Securities.Branches.Create;
using SharedComponent.Pages.Securities.Branches.Edit;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.UserBranches;
namespace SharedComponent.Pages.Securities.Branches;
public class BranchListBase : ComponentBase
{
    [Inject] private IUserBranchService BranchService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<UserBranchDetail>? BranchDetails { get; set; }
    [Inject]
    public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
    }

    private async Task GetData()
    {
        BranchDetails = await BranchService.GetUserBranch();
    }
    protected async Task OnAdd()
    {
        IsSpinner=true;
        await DialogService.OpenAsync<CreateBranch>("",
            new Dictionary<string, object>(),
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:80px; " });
        await GetData();
        IsSpinner=false;
        StateHasChanged();
    }

    protected async Task OnEdit(UserBranchDetail lab)
    {
        await DialogService.OpenAsync<EditBranch>("",
            new Dictionary<string, object>() { { "Model", lab } },
            new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:80px; " });
        await GetData();
        StateHasChanged();
    }
    protected async Task OnDelete(string id)
    {
        try
        {
            var confirm
                = await DialogService.Confirm("Are you sure?", "Delete",
                    new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirm.Equals(true))
            {
                var result = await BranchService.DeleteUserBranch(id);
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