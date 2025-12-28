using FleetManagement.Api.Authorization;
using FleetManagement.Application.RuleSets.Commands.ActivateRuleSet;
using FleetManagement.Application.RuleSets.Commands.CreateRuleSet;
using FleetManagement.Application.RuleSets.Queries.GetRuleSets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.RequireOrgAdminRole)]
public class RuleSetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RuleSetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRuleSet([FromBody] CreateRuleSetCommand command)
    {
        var tenantId = User.GetTenantId();
        var commandWithTenant = command with { TenantId = tenantId };
        var result = await _mediator.Send(commandWithTenant);
        return CreatedAtAction(nameof(GetRuleSets), result);
    }

    [HttpGet]
    public async Task<IActionResult> GetRuleSets()
    {
        var tenantId = User.GetTenantId();
        var query = new GetRuleSetsQuery(tenantId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateRuleSet(Guid id)
    {
        var tenantId = User.GetTenantId();
        var command = new ActivateRuleSetCommand(id, tenantId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
