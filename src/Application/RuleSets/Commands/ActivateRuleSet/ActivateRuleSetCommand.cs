using FleetManagement.Application.RuleSets.DTOs;
using MediatR;

namespace FleetManagement.Application.RuleSets.Commands.ActivateRuleSet;

public record ActivateRuleSetCommand(Guid Id, Guid TenantId) : IRequest<RuleSetDto>;
