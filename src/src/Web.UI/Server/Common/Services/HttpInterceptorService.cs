using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;
using Web.UI.Server.Api.Contracts;
using Web.UI.Server.Common.Services;
using Web.UI.Server.Constants;
using Web.UI.Server.Models.Security;
using Web.UI.StateManagement;

namespace BlazorProducts.Client.HttpRepository
{
	public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly RefreshTokenService _refreshTokenService;
		private readonly IAuthenticationService _authenticationService;
	  private  AuthenticationStateService _authProvider;
	  private  RefreshTokenQuery _query { get; set; }
		public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, IAuthenticationService authenticationService, AuthenticationStateService authProvider)
		{
			_interceptor = interceptor;
			_refreshTokenService = refreshTokenService;
			_authenticationService = authenticationService;
			_authProvider = authProvider;
		}

		public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

		public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
		{
			var absPath = e.Request.RequestUri.AbsolutePath; 
             
			if (!absPath.Contains("Account") && !absPath.Contains("refresh-token"))
			{
				_query=_authProvider.TokenQueries;
				var user = await _refreshTokenService.TryRefreshToken();

				if(user!=null)
				{
					e.Request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationSchemes.Bearer, user.Token);
				}
	
			}
		
		}

		public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
	}
}
