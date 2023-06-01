namespace Web.UI.StateManagement;

public class NavigationStateService
{
    public event Action? OnHeaderTabIndexChanged;
    public event Action? OnFooterTabIndexChanged;
    public event Action? OnSettingTabIndexChanged;
    public int HeaderTabIndex { get; private set; } = 1;
    public int SettingTabIndex { get; private set; } = 1;
    public int FooterTabIndex { get; private set; } 
    public void SetHeaderTabIndex(int index)
    {
        if (HeaderTabIndex == index)
        {
            return;
        }
        HeaderTabIndex = index;
        NotifyHeaderTabIndexChanged();
    }
    public void SetFooterTabIndex(int index)
    {
        if (FooterTabIndex == index)
        {
            return;
        }
        FooterTabIndex = index;
        NotifyFooterTabIndexChanged();
    }
    public void SetSettingTabIndex(int index)
    {
        if (SettingTabIndex == index)
        {
            return;
        }
        SettingTabIndex = index;
        NotifySettingTabIndexChanged();
    }
    private void NotifyHeaderTabIndexChanged() => OnHeaderTabIndexChanged?.Invoke();
    private void NotifyFooterTabIndexChanged() => OnFooterTabIndexChanged?.Invoke();
    private void NotifySettingTabIndexChanged() => OnSettingTabIndexChanged?.Invoke();
}