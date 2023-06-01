using System.Text;
using Microsoft.AspNetCore.Components;
using Health.Mobile.Server.Common;
using Health.Mobile.Server.Models.PatientFiles;

namespace Health.Mobile.Pages.LabTests.TemporaryFileUploads;

public class PatientFileDisplayBase: ComponentBase
{
    [Parameter] public IList<PatientFileDetail>? Data { get; set; }
    [Inject] private  ApiSetting _apiSetting{ get; set; } = null!;
    protected PatientFileDetail File = new();
    protected string Url = "";
    protected override async Task OnInitializedAsync()
    {
        File = Data?.FirstOrDefault();
        Url = new StringBuilder().Append(_apiSetting.BaseUrl).Append("/FileSave/TemporaryPatient/").ToString();
    }
}