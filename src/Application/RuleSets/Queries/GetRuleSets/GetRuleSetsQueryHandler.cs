using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.RuleSets.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.RuleSets.Queries.GetRuleSets;

public class GetRuleSetsQueryHandler : IRequestHandler<GetRuleSetsQuery, IEnumerable<RuleSetDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRuleSetsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RuleSetDto>> Handle(GetRuleSetsQuery request, CancellationToken cancellationToken)
    {
        var ruleSets = await _context.RuleSets
            .Where(r => r.TenantId == request.TenantId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return ruleSets.Select(r => new RuleSetDto(
            r.Id,
            r.TenantId,
            r.Name,
            r.Description,
            r.IsActive,
            r.CommissionPercentage,
            r.FixedFee?.Amount,
            r.FixedFee?.Currency,
            r.CreatedAtUtc,
            r.UpdatedAtUtc
        ));
    }
}
