namespace FleetManagement.Application.RuleSets.DTOs;

public record RuleSetDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string Description,
    bool IsActive,
    decimal CommissionPercentage,
    decimal? FixedFeeAmount,
    string? FixedFeeCurrency,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
