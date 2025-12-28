using FleetManagement.Application.PaySchedules.DTOs;
using FleetManagement.Domain.Enums;
using MediatR;

namespace FleetManagement.Application.PaySchedules.Commands.CreatePaySchedule;

public record CreatePayScheduleCommand(
    Guid TenantId,
    PayScheduleFrequency Frequency,
    DayOfWeek? DayOfWeek,
    int? DayOfMonth
) : IRequest<PayScheduleDto>;
