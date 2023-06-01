using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using SharedComponent.Server.Api.Contracts;
using SharedComponent.Server.Api.Impl;
using SharedComponent.Server.Common.Services;
using SharedComponent.Server.Extensions;
using SharedComponent.StateManagement;
using Microsoft.Extensions.DependencyInjection;

namespace SharedComponent.Server
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, string baseAddress)
        {
            services.TryAddSingleton<LabTestCategoryStateService>();
            services.TryAddSingleton<NavigationStateService>();
            services.TryAddSingleton<AuthenticationStateService>();
            services.TryAddSingleton<NotificationStateService>();
            services.AddHttpClient<ILabService, LabService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });

            services.AddHttpClient<ILabTestService, LabTestService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ILabCategoryService, LabCategoryService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ITestPriceService, TestPriceService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ISampleTypeService, SampleTypeService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ITubeTypeService, TubeTypeService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ISelectedTestDetailService, SelectedTestDetailService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ISelectedTestStatusService, SelectedTestStatusService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<ITestResultService, TestResultService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IUserAssignService, UserAssignService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IUserBranchService, UserBranchService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IPaymentService, PaymentService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IPatientFileService, PatientFileService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            }); 
            services.AddHttpClient<IOrganizationService, OrganizationService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IBankAccountService, BankAccountService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
            services.AddHttpClient<IFileSaverService, FileSaverService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });
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
            services.AddOptions();
            services.AddAuthorizationCore();
            return services;
        }
    }
}