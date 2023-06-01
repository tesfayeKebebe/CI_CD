
using Microsoft.AspNetCore.Components;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Models.LabTests;
using Web.UI.StateManagement;


namespace Web.UI.Pages.LabTests
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] private ILabTestService _labTestService { get; set; }
        [Inject] private LabTestCategoryStateService _labTestCategoryStateServiceService { get; set; } = null!;
        protected IList<LabTestCategory>? Data { get; set; }
        protected IList<LabTestCategory> Originaldata { get; set; }
        protected string? _search { get; set; }
        protected bool _clearEnabled { get; set; }
        
        protected HashSet<string> SelectedLabTests = new HashSet<string>();
        protected override async Task OnInitializedAsync()
        {

        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {

            SelectedLabTests = _labTestCategoryStateServiceService.SelectedLabTests;
            Originaldata   = await _labTestService.GetLabCategoryTest();
            Data = Originaldata;
            await InvokeAsync(StateHasChanged);
        }
        
        protected void OnInputChanged(ChangeEventArgs obj)
        {
            _clearEnabled = !string.IsNullOrEmpty(obj.Value?.ToString());
            _search = obj.Value?.ToString();
            SearchAsync();
            StateHasChanged();
        }
        protected void Clear()
        {
            _search = null;
            _clearEnabled = false;
            Data = Originaldata.ToList();
        }

        protected void SearchAsync()
        {
            if(_search==null) return;
            Data = new List<LabTestCategory>();
            foreach (var cat in Originaldata)
            {
                LabTestCategory labTest = new LabTestCategory();
                labTest.Id = cat.Id;
                labTest.Name = cat.Name;
                labTest.Lab = cat.Lab;
                labTest.LabTests = new List<LabTestPriceDetail>();
                foreach (var test in cat.LabTests.Where(x=>_search != null && x.Name.ToLower().Contains(_search.ToLower() )).ToList())
                {
                    labTest.LabTests.Add(test);
                }
                Data.Add(labTest);
            }
        }



        public void Dispose()
        {

        }
    }
}
