using FleetManagement.Api.Authorization;
using FleetManagement.Application.Tenants.Commands.CreateTenant;
using FleetManagement.Application.Tenants.Commands.UpdateCurrentTenant;
using FleetManagement.Application.Tenants.Queries.GetCurrentTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new tenant (SuperAdmin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireSuperAdminRole)]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCurrentTenant), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get current tenant information
    /// </summary>
    [HttpGet("me")]
    [Authorize(Policy = AuthorizationPolicies.RequireOrgAdminRole)]
    public async Task<IActionResult> GetCurrentTenant()
    {
        var tenantId = User.GetTenantId();
        var query = new GetCurrentTenantQuery(tenantId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Update current tenant information (OrgAdmin only)
    /// </summary>
    [HttpPut("me")]
    [Authorize(Policy = AuthorizationPolicies.RequireOrgAdminRole)]
    public async Task<IActionResult> UpdateCurrentTenant([FromBody] UpdateCurrentTenantCommand command)
    {
        var tenantId = User.GetTenantId();
        var commandWithTenantId = command with { TenantId = tenantId };
        var result = await _mediator.Send(commandWithTenantId);
        return Ok(result);
    }
}
