using FleetManagement.Api.Authorization;
using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.Tenants.Commands.CreateTenant;
using FleetManagement.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FleetManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Register IApplicationDbContext
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<FleetManagementDbContext>());

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateTenantCommand>();
    cfg.AddOpenBehavior(typeof(FleetManagement.Application.Common.Behaviors.ValidationBehavior<,>));
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantCommand>();

// Add controllers
builder.Services.AddControllers();

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Postgres")!);

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fleet Management API",
        Version = "v1",
        Description = "API for managing fleet operations, shifts, payouts, and drivers"
    });

    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
});

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
    options.AddPolicy(AuthorizationPolicies.RequireSuperAdminRole, policy =>
        policy.RequireRole("SuperAdmin"));

    options.AddPolicy(AuthorizationPolicies.RequireOrgAdminRole, policy =>
        policy.RequireRole("OrgAdmin"));

    options.AddPolicy(AuthorizationPolicies.RequireDriverRole, policy =>
        policy.RequireRole("Driver"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fleet Management API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();

public partial class Program { }