using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.Tenants.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.Tenants.Commands.UpdateCurrentTenant;

public class UpdateCurrentTenantCommandHandler : IRequestHandler<UpdateCurrentTenantCommand, TenantDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateCurrentTenantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TenantDto> Handle(UpdateCurrentTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
        }

        tenant.UpdateName(request.Name);

        await _context.SaveChangesAsync(cancellationToken);

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.IsActive,
            tenant.CreatedAtUtc,
            tenant.UpdatedAtUtc
        );
    }
}
