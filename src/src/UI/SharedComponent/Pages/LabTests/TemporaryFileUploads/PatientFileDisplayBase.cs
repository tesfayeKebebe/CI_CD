using System.Text;
using Microsoft.AspNetCore.Components;
using SharedComponent.Server.Common;
using SharedComponent.Server.Constants;
using SharedComponent.Server.Models.PatientFiles;

namespace SharedComponent.Pages.LabTests.TemporaryFileUploads;

public class PatientFileDisplayBase: ComponentBase
{
    [Parameter] public IList<PatientFileDetail>? Data { get; set; }
    protected PatientFileDetail File = new();
    protected string Url = "";
    protected override async Task OnInitializedAsync()
    {
        File = Data?.FirstOrDefault();
        Url = new StringBuilder().Append(ApiPath.BaseUrl).Append("/api/FileSave/TemporaryPatient/").ToString();
    }
}