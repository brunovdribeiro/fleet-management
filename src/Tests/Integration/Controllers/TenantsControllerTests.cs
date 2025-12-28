using System.Net;
using System.Net.Http.Json;
using FleetManagement.Application.Tenants.Commands.CreateTenant;
using FleetManagement.Application.Tenants.Commands.UpdateCurrentTenant;
using FleetManagement.Application.Tenants.DTOs;
using FluentAssertions;

namespace FleetManagement.Tests.Integration.Controllers;

public class TenantsControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateTenant_ShouldReturnCreated_WhenUserIsSuperAdmin()
    {
        // Arrange
        Authenticate(Guid.NewGuid(), role: "SuperAdmin");
        var command = new CreateTenantCommand("New Test Tenant");

        // Act
        var response = await Client.PostAsJsonAsync("/api/Tenants", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task CreateTenant_ShouldReturnForbidden_WhenUserIsNotSuperAdmin()
    {
        // Arrange
        Authenticate(Guid.NewGuid(), role: "OrgAdmin");
        var command = new CreateTenantCommand("Unauthorized Tenant");

        // Act
        var response = await Client.PostAsJsonAsync("/api/Tenants", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetCurrentTenant_ShouldReturnOk_WhenUserIsOrgAdmin()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant for Get");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");

        // Act
        var response = await Client.GetAsync("/api/Tenants/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(tenantId);
        result.Name.Should().Be("Test Tenant for Get");
    }

    [Fact]
    public async Task UpdateCurrentTenant_ShouldReturnOk_WhenUserIsOrgAdmin()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Original Name");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        var command = new UpdateCurrentTenantCommand(tenantId, "Updated Name");

        // Act
        var response = await Client.PutAsJsonAsync("/api/Tenants/me", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TenantDto>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(tenantId);
        result.Name.Should().Be("Updated Name");
    }

    private async Task<Guid> CreateTestTenantAsync(string name)
    {
        Authenticate(Guid.NewGuid(), role: "SuperAdmin");
        var command = new CreateTenantCommand(name);
        var response = await Client.PostAsJsonAsync("/api/Tenants", command);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TenantDto>();
        return result!.Id;
    }
}
