using System.Net;
using System.Net.Http.Json;
using FleetManagement.Application.PaySchedules.Commands.CreatePaySchedule;
using FleetManagement.Application.PaySchedules.Commands.UpdatePaySchedule;
using FleetManagement.Application.PaySchedules.DTOs;
using FleetManagement.Application.Tenants.Commands.CreateTenant;
using FleetManagement.Application.Tenants.DTOs;
using FleetManagement.Domain.Enums;
using FluentAssertions;

namespace FleetManagement.Tests.Integration.Controllers;

public class PaySchedulesControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task CreatePaySchedule_ShouldReturnCreated()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant PaySchedule");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        var command = new CreatePayScheduleCommand(
            tenantId,
            PayScheduleFrequency.Weekly,
            DayOfWeek.Monday,
            null
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/pay-schedule", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<PayScheduleDto>();
        result.Should().NotBeNull();
        result!.Frequency.Should().Be(command.Frequency);
        result.DayOfWeek.Should().Be(command.DayOfWeek);
    }

    [Fact]
    public async Task GetPaySchedule_ShouldReturnOk()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant PaySchedule Get");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        await CreatePayScheduleAsync(tenantId, PayScheduleFrequency.Monthly, null, 15);

        // Act
        var response = await Client.GetAsync("/api/pay-schedule");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PayScheduleDto>();
        result.Should().NotBeNull();
        result!.Frequency.Should().Be(PayScheduleFrequency.Monthly);
        result.DayOfMonth.Should().Be(15);
    }

    [Fact]
    public async Task UpdatePaySchedule_ShouldReturnOk()
    {
        // Arrange
        var tenantId = await CreateTestTenantAsync("Test Tenant PaySchedule Update");
        Authenticate(Guid.NewGuid(), tenantId, role: "OrgAdmin");
        
        await CreatePayScheduleAsync(tenantId, PayScheduleFrequency.Weekly, DayOfWeek.Monday, null);

        var command = new UpdatePayScheduleCommand(
            tenantId,
            PayScheduleFrequency.BiWeekly,
            DayOfWeek.Friday,
            null
        );

        // Act
        var response = await Client.PutAsJsonAsync("/api/pay-schedule", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PayScheduleDto>();
        result.Should().NotBeNull();
        result!.Frequency.Should().Be(PayScheduleFrequency.BiWeekly);
        result.DayOfWeek.Should().Be(DayOfWeek.Friday);
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

    private async Task<PayScheduleDto> CreatePayScheduleAsync(Guid tenantId, PayScheduleFrequency frequency, DayOfWeek? dayOfWeek, int? dayOfMonth)
    {
        var command = new CreatePayScheduleCommand(tenantId, frequency, dayOfWeek, dayOfMonth);
        var response = await Client.PostAsJsonAsync("/api/pay-schedule", command);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PayScheduleDto>())!;
    }
}
