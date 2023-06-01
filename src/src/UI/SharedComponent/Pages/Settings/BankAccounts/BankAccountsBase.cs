using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.BankAccounts.Creates;
using SharedComponent.Pages.Settings.BankAccounts.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.BankAccounts;
using SharedComponent.Server.Models.Categories;

namespace SharedComponent.Pages.Settings.BankAccounts;

public class BankAccountsBase : ComponentBase
{
    [Inject] private IBankAccountService BankAccountService { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;
    [Inject] private NotificationService NotificationService { get; set; } = null!;
    protected IList<BankAccountDetail> BankAccountDetails { get; set; } = new List<BankAccountDetail>();
    [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;
    protected bool IsSpinner = true;
    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
        IsSpinner=false;
    }

    private async Task GetData()
    {
        BankAccountDetails = await BankAccountService.GetBankAccounts();
    }

    protected async Task OnAdd()
    {
        await DialogService.OpenAsync<CreateBankAccount>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(BankAccountDetail bankAccount)
    {
        await DialogService.OpenAsync<EditBankAccount>("",
            new Dictionary<string, object>() {{"Model", bankAccount}},
            new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px;"});
        await GetData();
        StateHasChanged();
    }


}