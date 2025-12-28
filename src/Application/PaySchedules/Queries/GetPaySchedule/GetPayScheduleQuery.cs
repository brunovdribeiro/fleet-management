using FleetManagement.Application.PaySchedules.DTOs;
using MediatR;

namespace FleetManagement.Application.PaySchedules.Queries.GetPaySchedule;

public record GetPayScheduleQuery(Guid TenantId) : IRequest<PayScheduleDto>;
