using FleetManagement.Domain.Enums;

namespace FleetManagement.Application.PaySchedules.DTOs;

public record PayScheduleDto(
    Guid Id,
    Guid TenantId,
    PayScheduleFrequency Frequency,
    DayOfWeek? DayOfWeek,
    int? DayOfMonth,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
