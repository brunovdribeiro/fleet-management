using FleetManagement.Application.RuleSets.DTOs;
using MediatR;

namespace FleetManagement.Application.RuleSets.Commands.CreateRuleSet;

public record CreateRuleSetCommand(
    Guid TenantId,
    string Name,
    string Description,
    decimal CommissionPercentage,
    decimal? FixedFeeAmount,
    string? FixedFeeCurrency
) : IRequest<RuleSetDto>;
