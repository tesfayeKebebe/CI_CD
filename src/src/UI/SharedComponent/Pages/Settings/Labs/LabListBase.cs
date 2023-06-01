using Microsoft.AspNetCore.Components;
using Radzen;
using SharedComponent.Pages.Settings.Labs.Creates;
using SharedComponent.Pages.Settings.Labs.Edits;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Models.Labs;

namespace SharedComponent.Pages.Settings.Labs
{
    public class LabListBase: ComponentBase
    {
        [Inject] private ILabService LabService { get; set; } = null!;
        [Inject] private  DialogService DialogService { get; set; } = null!;
        [Inject] private NotificationService NotificationService { get; set; } = null!;
        protected IList<LabDetail>? Labs { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; } = null!;
        protected bool IsSpinner = true;
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
          await  GetData();
            IsSpinner=false;
        }

        private async Task GetData()
        {
            Labs = await LabService.GetLab(); 
        }
        protected async Task  OnAdd()
        {
            await DialogService.OpenAsync<Create>("",
                new Dictionary<string, object>(),
                new DialogOptions {Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
            await GetData();
            StateHasChanged();
        }

        protected async Task OnEdit(LabDetail lab)
        {
            await DialogService.OpenAsync<EditLab>("",
                new Dictionary<string, object>() { {"Model",lab} },  
                new DialogOptions { Width = "90%", Height = "100%", Draggable = true, Resizable = true,Style = "position: absolute; Top:80px; "});
            await GetData();
            StateHasChanged();
        }
       protected async Task  OnDelete(string id)
        {
            try
            {
                var confirm
                    =  await DialogService.Confirm("Are you sure?", "Delete",
                        new ConfirmOptions() {OkButtonText = "Yes", CancelButtonText = "No"});
                if (confirm.Equals(true))
                {
                    IsSpinner=true;
                    var result= await LabService.DeleteLab(id);
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
}
