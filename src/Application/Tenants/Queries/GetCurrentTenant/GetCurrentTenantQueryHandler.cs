using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.Tenants.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Application.Tenants.Queries.GetCurrentTenant;

public class GetCurrentTenantQueryHandler : IRequestHandler<GetCurrentTenantQuery, TenantDto>
{
    private readonly IApplicationDbContext _context;

    public GetCurrentTenantQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TenantDto> Handle(GetCurrentTenantQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant == null)
        {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
        }

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.IsActive,
            tenant.CreatedAtUtc,
            tenant.UpdatedAtUtc
        );
    }
}
