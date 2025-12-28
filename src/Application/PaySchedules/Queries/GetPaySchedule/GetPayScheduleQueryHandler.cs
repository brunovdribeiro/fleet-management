using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.PaySchedules.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.PaySchedules.Queries.GetPaySchedule;

public class GetPayScheduleQueryHandler : IRequestHandler<GetPayScheduleQuery, PayScheduleDto>
{
    private readonly IApplicationDbContext _context;

    public GetPayScheduleQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PayScheduleDto> Handle(GetPayScheduleQuery request, CancellationToken cancellationToken)
    {
        var paySchedule = await _context.PaySchedules
            .FirstOrDefaultAsync(p => p.TenantId == request.TenantId, cancellationToken);

        if (paySchedule == null)
        {
            throw new InvalidOperationException($"No pay schedule found for tenant {request.TenantId}");
        }

        return new PayScheduleDto(
            paySchedule.Id,
            paySchedule.TenantId,
            paySchedule.Frequency,
            paySchedule.DayOfWeek,
            paySchedule.DayOfMonth,
            paySchedule.CreatedAtUtc,
            paySchedule.UpdatedAtUtc
        );
    }
}
