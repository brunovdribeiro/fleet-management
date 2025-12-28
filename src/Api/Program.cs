using FleetManagement.Api.Authorization;
using FleetManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FleetManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authConfig = builder.Configuration.GetSection("Authentication");

        options.Authority = authConfig["Authority"];
        options.Audience = authConfig["Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = authConfig.GetValue<bool>("ValidateIssuer"),
            ValidateAudience = authConfig.GetValue<bool>("ValidateAudience"),
            ValidateLifetime = authConfig.GetValue<bool>("ValidateLifetime"),
            ValidateIssuerSigningKey = true
        };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.RequireOrgAdminRole, policy =>
        policy.RequireRole("OrgAdmin"));

    options.AddPolicy(AuthorizationPolicies.RequireDriverRole, policy =>
        policy.RequireRole("Driver"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello, World!");

app.Run();