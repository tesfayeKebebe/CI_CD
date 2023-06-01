
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Application;
using Application.Notifications;
using Web.API;
using Web.API.Helpers;
using Microsoft.Net.Http.Headers;

var myPolicy = "policy";
var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:myPolicy, policy =>
    {
        policy.WithOrigins("https://localhost:7221",
            "https://localhost:5192",
             "http://localhost:5192",
            "http://196.189.119.57",
           "http://www.yegnatena.com")
         .AllowAnyMethod()
        .AllowAnyHeader();
         
    });
});
// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - Web API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{IdentityServerConfig.ApiFriendlyName} V1");
        c.OAuthClientId(IdentityServerConfig.SwaggerClientID);
        c.OAuthClientSecret("no_password");
        //Leaving it blank doesn't work
    });
}
app.MapHub<SelectedLabTestHub>("/SelectedLabTestHub");
app.MapHub<DraftHub>("/DraftHub");
app.MapHub<PatientFileHub>("/PatientFileHub");
app.UseRouting();
//app.UseHttpsRedirection();
app.UseCors(myPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
        databaseInitializer.SeedAsync().Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);
        var loggerFactory = app.Services.GetService<ILoggerFactory>();
        loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
    }
}

app.Run();


