using RazorShared.Server.Models.Security;

namespace RazorShared.StateManagement;

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