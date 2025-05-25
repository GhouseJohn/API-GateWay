using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


// --- Add YARP Services ---
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// --- End YARP Services ---

// --- Add Health Checks UI Services ---
builder.Services.AddHealthChecksUI(setupSettings: setup =>
{
    // You can expose an API endpoint for the UI data itself
    setup.AddHealthCheckEndpoint("Gateway UI API", "/healthchecks-api");
    //setup.AddHealthCheckEndpoint("Catalog Service", "http://catalog.api:8080/health");
    //setup.AddHealthCheckEndpoint("Basket Service", "http://basket.api:8080/health");

}).AddInMemoryStorage(); // For production, use .AddSqlServerStorage(), .AddPostgreSqlStorage(), etc.
// --- End Health Checks UI Services ---

// Optional: Add health checks for the Gateway itself (separate from the UI monitoring)
builder.Services.AddHealthChecks()
    .AddCheck("Gateway Self Check", () => HealthCheckResult.Healthy("YARP Gateway is alive."));

//rate Limited
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});


var app = builder.Build();
app.UseRouting(); // YARP needs routing

//app.UseAuthorization();
// --- Map YARP Reverse Proxy Middleware ---
app.MapReverseProxy();
// --- End YARP Reverse Proxy Middleware ---

// --- Map Health Checks UI Endpoints ---
// This maps the UI dashboard endpoint
app.MapHealthChecksUI(setup =>
{
    setup.UIPath = "/healthchecks-dashboard"; // The URL path to access the UI (e.g., http://localhost:port/healthchecks-dashboard)
    setup.ApiPath = "/healthchecks-api";      // The internal API path for the UI to fetch data
});
// Optional: Expose the Gateway's own health check endpoint
app.MapHealthChecks("/gateway-health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
// --- End Health Checks UI Endpoints ---

app.Run();
