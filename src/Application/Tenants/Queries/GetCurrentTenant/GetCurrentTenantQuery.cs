using FleetManagement.Application.Tenants.DTOs;
using MediatR;

namespace FleetManagement.Application.Tenants.Queries.GetCurrentTenant;

public record GetCurrentTenantQuery(Guid TenantId) : IRequest<TenantDto>;
