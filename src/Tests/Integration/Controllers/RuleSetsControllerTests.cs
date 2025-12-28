using System.Net;
using System.Net.Http.Json;
using FleetManagement.Application.RuleSets.Commands.CreateRuleSet;
using FleetManagement.Application.RuleSets.DTOs;
using FleetManagement.Application.Tenants.Commands.CreateTenant;
using FleetManagement.Application.Tenants.DTOs;
using FluentAssertions;

namespace FleetManagement.Tests.Integration.Controllers;

public class RuleSetsControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateRuleSet_ShouldReturnCreated()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant Ruleset");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        var command = new CreateRuleSetCommand(
            tenantId,
            "Standard Ruleset",
            "Description",
            10.5m,
            5.0m,
            "USD"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/RuleSets", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<RuleSetDto>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(command.Name);
        result.CommissionPercentage.Should().Be(command.CommissionPercentage);
        result.FixedFeeAmount.Should().Be(command.FixedFeeAmount);
        result.FixedFeeCurrency.Should().Be(command.FixedFeeCurrency);
    }

    [Fact]
    public async Task GetRuleSets_ShouldReturnList()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant Ruleset List");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        await CreateRuleSetAsync(tenantId, "Ruleset 1");
        await CreateRuleSetAsync(tenantId, "Ruleset 2");

        // Act
        var response = await Client.GetAsync("/api/RuleSets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<RuleSetDto>>();
        result.Should().NotBeNull();
        result!.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task ActivateRuleSet_ShouldReturnOk()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant Ruleset Activate");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        var ruleSet = await CreateRuleSetAsync(tenantId, "Ruleset to Activate");

        // Act
        var response = await Client.PostAsync($"/api/RuleSets/{ruleSet.Id}/activate", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RuleSetDto>();
        result.Should().NotBeNull();
        result!.IsActive.Should().BeTrue();
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

    private async Task<RuleSetDto> CreateRuleSetAsync(Guid tenantId, string name)
    {
        var command = new CreateRuleSetCommand(tenantId, name, "Desc", 10m, null, null);
        var response = await Client.PostAsJsonAsync("/api/RuleSets", command);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RuleSetDto>())!;
    }
}
