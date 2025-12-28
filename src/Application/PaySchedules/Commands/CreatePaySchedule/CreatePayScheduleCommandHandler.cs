using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.PaySchedules.DTOs;
using FleetManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.PaySchedules.Commands.CreatePaySchedule;

public class CreatePayScheduleCommandHandler : IRequestHandler<CreatePayScheduleCommand, PayScheduleDto>
{
    private readonly IApplicationDbContext _context;

    public CreatePayScheduleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PayScheduleDto> Handle(CreatePayScheduleCommand request, CancellationToken cancellationToken)
    {
        var existingSchedule = await _context.PaySchedules
            .FirstOrDefaultAsync(p => p.TenantId == request.TenantId, cancellationToken);

        if (existingSchedule != null)
        {
            throw new InvalidOperationException("A pay schedule already exists for this tenant. Use PUT to update it.");
        }

        var paySchedule = new PaySchedule(
            Guid.NewGuid(),
            request.TenantId,
            request.Frequency,
            request.DayOfWeek,
            request.DayOfMonth
        );

        _context.PaySchedules.Add(paySchedule);
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
