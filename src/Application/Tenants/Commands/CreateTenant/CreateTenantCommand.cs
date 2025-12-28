using FleetManagement.Application.Tenants.DTOs;
using MediatR;

namespace FleetManagement.Application.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(string Name) : IRequest<TenantDto>;
