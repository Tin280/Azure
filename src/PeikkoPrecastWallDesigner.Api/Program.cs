using System.Threading.RateLimiting;
using Azure.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using PeikkoPrecastWallDesigner.Application;
using PeikkoPrecastWallDesigner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

//builder.Configuration.AddAzureKeyVault(
//	new Uri($"https://{env.ApplicationName}.vault.azure.net/"),
//	new DefaultAzureCredential());
// ----------------------------------------------------
// SERVICES CONFIGURATION
// ----------------------------------------------------
var services = builder.Services;

services.AddApplication(builder.Configuration);
services.AddInfrastructure(builder);

services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
	options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
	options.AddPolicy("Fixed", httpContext =>
		RateLimitPartition.GetFixedWindowLimiter(
			partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
			factory: partition => new FixedWindowRateLimiterOptions
			{
				PermitLimit = 100,
				Window = TimeSpan.FromMinutes(1),
				QueueProcessingOrder = QueueProcessingOrder.OldestFirst
			}
	));
});

// ----------------------------------------------------
// BUILD AND RUN THE APPLICATION
// ----------------------------------------------------
var app = builder.Build();

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
