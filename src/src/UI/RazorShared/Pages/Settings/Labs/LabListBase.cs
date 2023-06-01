using Microsoft.AspNetCore.Components;
using Radzen;
using RazorShared.Pages.Settings.Labs.Creates;
using RazorShared.Pages.Settings.Labs.Edits;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Models.Labs;

namespace RazorShared.Pages.Settings.Labs
{
    public class LabListBase: ComponentBase
    {
        [Inject] private ILabService LabService { get; set; } = null!;
        [Inject] private  DialogService DialogService { get; set; } = null!;
        [Inject] private NotificationService NotificationService { get; set; } = null!;
        protected IList<LabDetail>? Labs { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; } = null!;
        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();
          await  GetData();
        }

        private async Task GetData()
        {
            Labs = await LabService.GetLab(); 
        }
        protected async Task  OnAdd()
        {
            await DialogService.OpenAsync<Create>("",
                new Dictionary<string, object>(),
                new DialogOptions { Width = "50%", Height = "570px" });
            await GetData();
            StateHasChanged();
        }

        protected async Task OnEdit(LabDetail lab)
        {
            await DialogService.OpenAsync<EditLab>("",
                new Dictionary<string, object>() { {"Model",lab} },  
                new DialogOptions { Width = "50%", Height = "570px"});
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
                    var result= await LabService.DeleteLab(id);
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
}
