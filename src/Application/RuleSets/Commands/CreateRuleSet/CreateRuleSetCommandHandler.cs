using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.RuleSets.DTOs;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.ValueObjects;
using MediatR;

namespace FleetManagement.Application.RuleSets.Commands.CreateRuleSet;

public class CreateRuleSetCommandHandler : IRequestHandler<CreateRuleSetCommand, RuleSetDto>
{
    private readonly IApplicationDbContext _context;

    public CreateRuleSetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RuleSetDto> Handle(CreateRuleSetCommand request, CancellationToken cancellationToken)
    {
        Money? fixedFee = null;
        if (request.FixedFeeAmount.HasValue && !string.IsNullOrWhiteSpace(request.FixedFeeCurrency))
        {
            fixedFee = new Money(request.FixedFeeAmount.Value, request.FixedFeeCurrency);
        }

        var ruleSet = new RuleSet(
            Guid.NewGuid(),
            request.TenantId,
            request.Name,
            request.Description,
            request.CommissionPercentage,
            fixedFee
        );

        _context.RuleSets.Add(ruleSet);
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
