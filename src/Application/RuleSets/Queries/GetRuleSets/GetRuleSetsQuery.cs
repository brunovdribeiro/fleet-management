using FleetManagement.Application.RuleSets.DTOs;
using MediatR;

namespace FleetManagement.Application.RuleSets.Queries.GetRuleSets;

public record GetRuleSetsQuery(Guid TenantId) : IRequest<IEnumerable<RuleSetDto>>;
