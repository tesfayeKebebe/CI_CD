using Blazored.LocalStorage;
using SharedComponent.Server.Constants;

namespace SharedComponent.Server.Common.Services
{
    public class SessionStoreService
    {
        private readonly ILocalStorageService _localStorage;

        public SessionStoreService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
        
        public async   Task<SessionStore> Get()
        {
            var sessionData = await _localStorage.GetItemAsync<SessionStore>(ApplicationConstant.SessionKey);
            if (sessionData?.Authentication != null && sessionData.Authentication.ExpiresAt >= DateTime.UtcNow)
            {
                return sessionData;
            }

            sessionData = new SessionStore();
       await  _localStorage.SetItemAsync(ApplicationConstant.SessionKey, sessionData);
            return sessionData;
        }

        public async Task  Set(SessionStore sessionData)
        {
           await _localStorage.SetItemAsync(ApplicationConstant.SessionKey, sessionData);
        }
        public async Task  RemoveAsync()
        {
            await _localStorage.RemoveItemAsync(ApplicationConstant.SessionKey);
        }

    }
}
