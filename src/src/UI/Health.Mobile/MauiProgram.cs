
using Microsoft.Extensions.Logging;
using Radzen;
//using MauiShared.Interfaces;
//using MauiShared.Imple;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Health.Mobile.Server.Common;
using Health.Mobile.Server;
using Health.Mobile.Server.Constants;

namespace Health.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
               //.UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            //builder.Services.AddSingleton<INetworkAcessService, NetworkAccessService>();
            builder.Services.TryAddConfiguration<ApiSetting>(builder.Configuration);
            builder.Services.AddScoped(sp => new HttpClient {}.EnableIntercept(sp));
            builder.Services.AddApiServices();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}