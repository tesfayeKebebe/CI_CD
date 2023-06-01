using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Health.Mobile.Server.Api.Contracts;
using Health.Mobile.Server.Constants;
using Health.Mobile.Server.Models.Security;
using Health.Mobile.StateManagement;
using Toolbelt.Blazor;

namespace Health.Mobile.Server.Common.Services
{
	public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly RefreshTokenService _refreshTokenService;
		private readonly IAuthenticationService _authenticationService;
	  private  AuthenticationStateService _authProvider;
	  private readonly NavigationManager _navigationManager;
	  public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, IAuthenticationService authenticationService, AuthenticationStateService authProvider, NavigationManager navigationManager)
		{
			_interceptor = interceptor;
			_refreshTokenService = refreshTokenService;
			_authenticationService = authenticationService;
			_authProvider = authProvider;
			_navigationManager = navigationManager;
		}

		public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

		private async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
		{
			if (e.Request.RequestUri != null)
			{
				var absPath = e.Request.RequestUri.AbsolutePath; 
             
				if (!absPath.Contains("Account") && !absPath.Contains("refresh-token"))
				{
			
					var user = await _refreshTokenService.TryRefreshToken();
					if(user!=null)
					{
						e.Request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, user.Token);
					}
					else
					{
						_navigationManager.NavigateTo("/home", true);
					}
	
				}
			}
		}

		public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
	}
}
