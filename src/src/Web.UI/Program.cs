using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Web.UI;
using Web.UI.Server;
using Web.UI.Server.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.TryAddConfiguration<ApiSetting>(builder.Configuration);
builder.Services.AddScoped(sp => new HttpClient {   BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)  }.EnableIntercept(sp));
builder.Services.AddApiServices();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
await builder.Build().RunAsync();
