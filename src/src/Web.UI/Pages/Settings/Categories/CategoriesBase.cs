using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.Categories.Creates;
using Web.UI.Pages.Settings.Categories.Edits;
using Web.UI.Pages.Settings.Labs.Creates;
using Web.UI.Pages.Settings.Labs.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Categories;
using Web.UI.Server.Models.Labs;

namespace Web.UI.Pages.Settings.Categories;

public class EditCategoryBase : ComponentBase
{
    [Inject] private ILabCategoryService labCategoryService { get; set; }
    [Inject] private DialogService _dialogService { get; set; }
    [Inject] private NotificationService _notificationService { get; set; }
    protected IList<CategoryDetail>? categoryDetails { get; set; }
    [Inject] public HttpInterceptorService Interceptor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetData();
    }

    private async Task GetData()
    {
        categoryDetails = await labCategoryService.GetLabCategory();
    }

    protected async Task OnAdd()
    {
        await _dialogService.OpenAsync<CreateLabCategory>("",
            new Dictionary<string, object>(),
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnEdit(CategoryDetail cat)
    {
        await _dialogService.OpenAsync<EditLabCategory>("",
            new Dictionary<string, object>() {{"model", cat}},
            new DialogOptions {Width = "50%", Height = "570px", Style = "position: absolute; Top:40px; "});
        await GetData();
        StateHasChanged();
    }

    protected async Task OnDelete(string id)
    {
        try
        {
            var result = await labCategoryService.DeleteLabCategory(id);
            _notificationService.Notify(NotificationSeverity.Success, "", result, 6000);
            await GetData();
            StateHasChanged();
        }
        catch (Exception e)
        {
            _notificationService.Notify(NotificationSeverity.Error, "", e.Message, 6000);
        }
    }
}