using System.Net.Http.Headers;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Constants;
using RazorShared.Server.Models.Security;
using RazorShared.StateManagement;
using Toolbelt.Blazor;

namespace RazorShared.Server.Common.Services
{
	public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly RefreshTokenService _refreshTokenService;
		private readonly IAuthenticationService _authenticationService;
	  private  AuthenticationStateService _authProvider;
	  public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, IAuthenticationService authenticationService, AuthenticationStateService authProvider)
		{
			_interceptor = interceptor;
			_refreshTokenService = refreshTokenService;
			_authenticationService = authenticationService;
			_authProvider = authProvider;
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
	
				}
			}
		}

		public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
	}
}
