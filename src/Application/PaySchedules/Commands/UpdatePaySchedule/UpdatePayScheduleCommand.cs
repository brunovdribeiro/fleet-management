using FleetManagement.Application.PaySchedules.DTOs;
using FleetManagement.Domain.Enums;
using MediatR;

namespace FleetManagement.Application.PaySchedules.Commands.UpdatePaySchedule;

public record UpdatePayScheduleCommand(
    Guid TenantId,
    PayScheduleFrequency Frequency,
    DayOfWeek? DayOfWeek,
    int? DayOfMonth
) : IRequest<PayScheduleDto>;
