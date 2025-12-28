using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.PaySchedules.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.PaySchedules.Commands.UpdatePaySchedule;

public class UpdatePayScheduleCommandHandler : IRequestHandler<UpdatePayScheduleCommand, PayScheduleDto>
{
    private readonly IApplicationDbContext _context;

    public UpdatePayScheduleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PayScheduleDto> Handle(UpdatePayScheduleCommand request, CancellationToken cancellationToken)
    {
        var paySchedule = await _context.PaySchedules
            .FirstOrDefaultAsync(p => p.TenantId == request.TenantId, cancellationToken);

        if (paySchedule == null)
        {
            throw new InvalidOperationException($"No pay schedule found for tenant {request.TenantId}. Use POST to create one.");
        }

        paySchedule.Update(request.Frequency, request.DayOfWeek, request.DayOfMonth);
        await _context.SaveChangesAsync(cancellationToken);

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
