using FleetManagement.Application.Common.Interfaces;
using FleetManagement.Application.Tenants.DTOs;
using FleetManagement.Domain.Entities;
using MediatR;

namespace FleetManagement.Application.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
{
    private readonly IApplicationDbContext _context;

    public CreateTenantCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Tenant(Guid.NewGuid(), request.Name);

        _context.Tenants.Add(tenant);
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
