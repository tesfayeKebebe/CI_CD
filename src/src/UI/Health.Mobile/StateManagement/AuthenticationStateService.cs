using Health.Mobile.Server.Models.Security;

namespace Health.Mobile.StateManagement;

public class AuthenticationStateService
{
    public event Action? OnIndentityChange;
    public RefreshTokenQuery TokenQueries { get; private set; } =new();
    public void SetAuthentication(RefreshTokenQuery query)
    {
        TokenQueries = query;
        NotifyAuthenticationChanged();
    }
    private void NotifyAuthenticationChanged() => OnIndentityChange?.Invoke();
}