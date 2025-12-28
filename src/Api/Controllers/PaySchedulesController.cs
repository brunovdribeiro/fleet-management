using FleetManagement.Api.Authorization;
using FleetManagement.Application.PaySchedules.Commands.CreatePaySchedule;
using FleetManagement.Application.PaySchedules.Commands.UpdatePaySchedule;
using FleetManagement.Application.PaySchedules.Queries.GetPaySchedule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Api.Controllers;

[ApiController]
[Route("api/pay-schedule")]
[Authorize(Policy = AuthorizationPolicies.RequireOrgAdminRole)]
public class PaySchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaySchedulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaySchedule()
    {
        var tenantId = User.GetTenantId();
        var query = new GetPayScheduleQuery(tenantId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaySchedule([FromBody] CreatePayScheduleCommand command)
    {
        var tenantId = User.GetTenantId();
        var commandWithTenant = command with { TenantId = tenantId };
        var result = await _mediator.Send(commandWithTenant);
        return CreatedAtAction(nameof(GetPaySchedule), result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePaySchedule([FromBody] UpdatePayScheduleCommand command)
    {
        var tenantId = User.GetTenantId();
        var commandWithTenant = command with { TenantId = tenantId };
        var result = await _mediator.Send(commandWithTenant);
        return Ok(result);
    }
}
