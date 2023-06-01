using System.Collections.Immutable;
using RazorShared.Pages.ViewModel;
using RazorShared.Server.Models.LabTests;

namespace RazorShared.StateManagement;

public class LabTestCategoryStateService
{
    public event Action? OnLabTestCategoryChanged;
    private readonly IDictionary<string, LabTestCategoryViewModel>
        _viewModels = new Dictionary<string, LabTestCategoryViewModel>();
    public  IDictionary<string, LabTestCategoryViewModel>
        ViewModels =>_viewModels;
    private readonly HashSet<string> _selectedLabTests = new();
    public HashSet<string> SelectedLabTests => _selectedLabTests;
    public  int TotalItem { get; set; }
    public  double TotalPrice { get; set; }

    public void AddLabTest(LabTestCategory labTest, LabTestPriceDetail test)
    {
        if (_viewModels.ContainsKey(labTest.Id))
        {
            foreach (var (_, cat) in _viewModels)
            {
                if (cat.Id != labTest.Id)
                {
                    continue;
                }
                if (!cat.LabTests.ContainsKey(test.Id))
                {
                    cat.LabTests.Add(test.Id,test);
                    _selectedLabTests.Add(test.Id);
                    TotalPrice += test.Price;
                    TotalItem++;
                }
            }
        }
        else
        {
            _viewModels.Add(labTest.Id,new LabTestCategoryViewModel
            {
                Id = labTest.Id,
                Lab = labTest.Lab,
                Name = labTest.Name,
            });
            foreach (var (_, cat) in _viewModels)
            {
                if (cat.Id != labTest.Id)
                {
                    continue;
                }
                if (!cat.LabTests.ContainsKey(test.Id))
                {
                    cat.LabTests.Add(test.Id,test);
                    TotalPrice += test.Price;
                    _selectedLabTests.Add(test.Id);
                    TotalItem++;
                }
            } 
        }
        NotifyLabTestChanged();
    }
    public void RemoveLabTest(LabTestPriceDetail test)
    {
        foreach (var (_, cat) in _viewModels)
        {
            if (!cat.LabTests.ContainsKey(test.Id))
            {
                continue;
            }

            cat.LabTests.Remove(test.Id);
            _selectedLabTests.Remove(test.Id);
            TotalPrice -= test.Price;
            TotalItem -= 1;
            if (cat.LabTests.Count==0)
            {
                _viewModels.Remove(cat.Id);
            }

        }
        
        NotifyLabTestChanged();
    }
    private void NotifyLabTestChanged() => OnLabTestCategoryChanged?.Invoke();

}