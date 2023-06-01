using System.Text;
using Application.Business;
using Application.Common.Interfaces;
using Domain.Identity;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Infrastructure.Identity;
using Infrastructure.Identity.Security;
using Infrastructure.Identity.Security.Authorization;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.API.Helpers;
using Web.API.Services;
using Web.UI.Filters;
//using NSwag;
//using NSwag.Generation.Processors.Security;
//using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;
using AppPermissions = Infrastructure.Identity.Security.ApplicationPermissions;
namespace Web.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();
         services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
         services.AddScoped<IEmailService, EmailService>();
         services.AddSwaggerGen(option =>
         {
             option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter your token in the text input below.\r\n",
                 In = ParameterLocation.Header,
                 Name = "Authorization",
                 Type = SecuritySchemeType.Http,
                 BearerFormat = "JWT",
                 Scheme = "Bearer"
             });

             option.AddSecurityRequirement(new OpenApiSecurityRequirement()
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                         }
                     },
                     new string[]{}
                 }
             });
         });


        //services.AddHealthChecks()
        //    .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        var authServerUrl =configuration["JWT:ValidIssuer"];
        // add identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(
                option =>
                {
                    option.User.RequireUniqueEmail = false;
                    //// Password settings
                    option.Password.RequireDigit = false;
                    option.Password.RequiredLength = 4;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireLowercase = false;
                })
            
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddPasswordValidator<PasswordValidation<ApplicationUser>>();

        // Configure Identity options and password complexity here
        services.Configure<IdentityOptions>(options =>
        {
        });

        // Adds IdentityServer.
        services.AddIdentityServer(o =>
        {
            o.IssuerUri = authServerUrl;
        })
          // The AddDeveloperSigningCredential extension creates temporary key material for signing tokens.
          // This might be useful to get started, but needs to be replaced by some persistent key material for production scenarios.
          // See http://docs.identityserver.io/en/release/topics/crypto.html#refcrypto for more information.
          .AddDeveloperSigningCredential()
          .AddInMemoryPersistedGrants()
          // To configure IdentityServer to use EntityFramework (EF) as the storage mechanism for configuration data (rather than using the in-memory implementations),
          // see https://identityserver4.readthedocs.io/en/release/quickstarts/8_entity_framework.html
          .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
          .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
          .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
          .AddInMemoryClients(IdentityServerConfig.GetClients())
          .AddAspNetIdentity<ApplicationUser>()
          .AddProfileService<ProfileService>();
        services.AddAuthentication(o =>
            {

                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.ViewAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ViewUsers));
            options.AddPolicy(Policies.ManageAllUsersPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ManageUsers));

            options.AddPolicy(Policies.ViewAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ViewRoles));
            options.AddPolicy(Policies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
            options.AddPolicy(Policies.ManageAllRolesPolicy, policy => policy.RequireClaim(ClaimConstants.Permission, AppPermissions.ManageRoles));

            options.AddPolicy(Policies.AssignAllowedRolesPolicy, policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
        });
        
        services.AddAutoMapper(typeof(Program));

        // Configurations
        services.Configure<AppSettings>(configuration);
        // Business Services
        services.AddScoped<IEmailSender, EmailSender>();
        return services;
    }



}
