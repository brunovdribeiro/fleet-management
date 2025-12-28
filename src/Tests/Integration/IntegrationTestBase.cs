using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using FleetManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Testcontainers.PostgreSql;

namespace FleetManagement.Tests.Integration;

public class IntegrationTestBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .Build();

    protected HttpClient Client { get; private set; } = null!;
    protected WebApplicationFactory<Program> Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        try 
        {
            await _dbContainer.StartAsync();
        }
        catch (Exception ex)
        {
            // If Docker is not available, we can't run these tests.
            // In a real CI/CD or local dev environment, Docker should be running.
            Console.WriteLine($"[DEBUG_LOG] Failed to start Testcontainer: {ex.Message}");
            throw;
        }

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Remove existing DbContext registration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FleetManagementDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Add DbContext using Testcontainer connection string
                    services.AddDbContext<FleetManagementDbContext>(options =>
                    {
                        options.UseNpgsql(_dbContainer.GetConnectionString());
                    });

                    // Add Mock Authentication
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                });
            });

        Client = Factory.CreateClient();
        
        // Ensure database is created and migrations are applied
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FleetManagementDbContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    protected void Authenticate(Guid userId, Guid? tenantId = null, string role = "OrgAdmin")
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        TestAuthHandler.UserId = userId;
        TestAuthHandler.TenantId = tenantId;
        TestAuthHandler.Role = role;
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public static Guid UserId { get; set; } = Guid.NewGuid();
    public static Guid? TenantId { get; set; }
    public static string Role { get; set; } = "OrgAdmin";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
            new Claim(ClaimTypes.Role, Role)
        };

        if (TenantId.HasValue)
        {
            claims.Add(new Claim("tenant_id", TenantId.Value.ToString()));
        }

        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
