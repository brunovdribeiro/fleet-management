using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.RuleSets.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.RuleSets.Commands.ActivateRuleSet;

public class ActivateRuleSetCommandHandler : IRequestHandler<ActivateRuleSetCommand, RuleSetDto>
{
    private readonly IApplicationDbContext _context;

    public ActivateRuleSetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RuleSetDto> Handle(ActivateRuleSetCommand request, CancellationToken cancellationToken)
    {
        var ruleSet = await _context.RuleSets
            .FirstOrDefaultAsync(r => r.Id == request.Id && r.TenantId == request.TenantId, cancellationToken);

        if (ruleSet == null)
        {
            throw new InvalidOperationException($"RuleSet with ID {request.Id} not found for this tenant");
        }

        ruleSet.Activate();
        await _context.SaveChangesAsync(cancellationToken);

        return new RuleSetDto(
            ruleSet.Id,
            ruleSet.TenantId,
            ruleSet.Name,
            ruleSet.Description,
            ruleSet.IsActive,
            ruleSet.CommissionPercentage,
            ruleSet.FixedFee?.Amount,
            ruleSet.FixedFee?.Currency,
            ruleSet.CreatedAtUtc,
            ruleSet.UpdatedAtUtc
        );
    }
}
