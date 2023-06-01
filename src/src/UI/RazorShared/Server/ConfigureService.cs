using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using RazorShared.Server.Api.Contracts;
using RazorShared.Server.Api.Impl;
using RazorShared.Server.Common.Services;
using RazorShared.Server.Extensions;
using RazorShared.StateManagement;
using Microsoft.Extensions.DependencyInjection;

namespace RazorShared.Server
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
             services.TryAddSingleton<LabTestCategoryStateService>();
            services.TryAddSingleton<NavigationStateService>();
            services.TryAddSingleton<AuthenticationStateService>();
            services.TryAddScoped<ILabService, LabService>();
            services.TryAddScoped<ILabTestService, LabTestService>();
            services.TryAddScoped<IAuthenticationService, AuthenticationService>();
            services.TryAddScoped<ILabCategoryService, LabCategoryService>();
            services.TryAddScoped<ITestPriceService, TestPriceService>();
            services.TryAddScoped<ISampleTypeService, SampleTypeService>();
            services.TryAddScoped<ITubeTypeService, TubeTypeService>();
            services.TryAddScoped<ISelectedTestDetailService, SelectedTestDetailService>();
            services.TryAddScoped<ISelectedTestStatusService, SelectedTestStatusService>();
            services.TryAddScoped<ITestResultService, TestResultService>();
            services.TryAddScoped<IUserAssignService, UserAssignService>();
            services.TryAddScoped<IUserBranchService, UserBranchService>();
            services.TryAddScoped<IPaymentService, PaymentService>();
            services.AddBlazoredLocalStorage(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonExtension.GetOptions().PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonExtension.GetOptions().DictionaryKeyPolicy;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonExtension.GetOptions().DefaultIgnoreCondition;
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = JsonExtension.GetOptions().IgnoreReadOnlyFields;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonExtension.GetOptions().PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonExtension.GetOptions().PropertyNamingPolicy;
                options.JsonSerializerOptions.ReadCommentHandling = JsonExtension.GetOptions().ReadCommentHandling;
                options.JsonSerializerOptions.WriteIndented = JsonExtension.GetOptions().WriteIndented;
            });
            services.AddScoped<RefreshTokenService>();
            services.TryAddScoped<SessionStoreService>();
            services.TryAddScoped<BearerAuthStateProvider>();
            services.AddScoped<HttpInterceptorService>();
            services.AddHttpClientInterceptor();
            services.TryAddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<BearerAuthStateProvider>());
            services.AddAuthorizationCore();
            return services;
        }
    }
}
