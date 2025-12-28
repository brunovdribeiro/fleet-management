using FluentValidation;

namespace FleetManagement.Application.Tenants.Commands.UpdateCurrentTenant;

public class UpdateCurrentTenantCommandValidator : AbstractValidator<UpdateCurrentTenantCommand>
{
    public UpdateCurrentTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Tenant name is required")
            .MaximumLength(200)
            .WithMessage("Tenant name cannot exceed 200 characters");
    }
}
