using BlazorProducts.Client.HttpRepository;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Pages.Settings.Labs.Creates;
using Web.UI.Pages.Settings.Labs.Edits;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.Labs;

namespace Web.UI.Pages.Settings.Labs
{
    public class LabListBase: ComponentBase
    {
        [Inject] private ILabService labService { get; set; }
        [Inject] private  DialogService _dialogService { get; set; }
        [Inject] private NotificationService _notificationService { get; set; }
        protected IList<LabDetail>? labs { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
          await  GetData();
        }

        private async Task GetData()
        {
            labs = await labService.GetLab(); 
        }
        protected async Task  OnAdd()
        {
            await _dialogService.OpenAsync<Create>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "300px", Height = "570px", Style = "position: absolute; Top:40px; " });
            await GetData();
            StateHasChanged();
        }

        protected async Task OnEdit(LabDetail lab)
        {
            await _dialogService.OpenAsync<EditLab>("",
                new Dictionary<string, object>() { {"model",lab} },  
                new DialogOptions { Width = "300px", Height = "570px", Style = "position: absolute; Top:40px; " });
            await GetData();
            StateHasChanged();
        }
       protected async Task  OnDelete(string id)
        {
            try
            {
           var result= await labService.DeleteLab(id);
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
}
