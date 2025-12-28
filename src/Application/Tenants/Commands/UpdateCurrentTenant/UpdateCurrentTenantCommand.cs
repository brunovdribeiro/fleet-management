using FleetManagement.Application.Tenants.DTOs;
using MediatR;

namespace FleetManagement.Application.Tenants.Commands.UpdateCurrentTenant;

public record UpdateCurrentTenantCommand(Guid TenantId, string Name) : IRequest<TenantDto>;
