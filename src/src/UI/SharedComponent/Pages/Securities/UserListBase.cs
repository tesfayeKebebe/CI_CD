using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Security;

namespace SharedComponent.Pages.Securities;

public class UserListBase: ComponentBase
{
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<ApplicationUser>? ApplicationUsers { get; set; } = null!;
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    private IList<ApplicationUser> OriginalData { get; set; } = null!;

    protected string? Search { get; set; }
    protected bool ClearEnabled { get; set; }
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
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
        ApplicationUsers = OriginalData.ToList();
    }

    private void SearchAsync()
    {
        if(Search==null) return;
        ApplicationUsers = new List<ApplicationUser>();
        foreach (var testPrice in OriginalData.Where(x=>x.UserName != null && Search != null && x.UserName.ToLower().Contains(Search.ToLower() )).ToList().Select(user => new ApplicationUser
                 {
                     Id = user.Id,
                     Email = user.Email,
                     Password = user.Password,
                     Roles = user.Roles,
                     UserName = user.UserName,
                     FullName = user.FullName,
                     IdNumber = user.IdNumber,
                     PhoneNumber = user.PhoneNumber

                 }))
        {
            ApplicationUsers.Add(testPrice);
        }
    }
    private async Task GetData()
    {
        ApplicationUsers = await AuthenticationService.GetAllUsers();
        OriginalData = ApplicationUsers;
    }
    protected async Task OnAdd()
    {
        var register = new RegisterUser
        {
            IsAdmin = true
        };
        await DialogService.OpenAsync<Register>("",
            new Dictionary<string, object>() {{"Model", register}},
            new DialogOptions {Width = "90%", Height = "90%",Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(ApplicationUser user)
    {
        var userEdit = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            IsEnabled = user.IsEnabled,
            Email = user.Email
        };
        await DialogService.OpenAsync<EditUser>("",
            new Dictionary<string, object>() {{"Model", userEdit}},
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true, Style = "position: absolute; Top:80px; "});
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
                IsSpinner=true;
                var result = await AuthenticationService.Delete(id);
                NotificationService.Notify(NotificationSeverity.Success, "", result, 6000);
               await GetData();
                IsSpinner=false;
               StateHasChanged();
            }
        }
        catch (Exception e)
        {
            NotificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }


    
}