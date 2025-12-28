using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Primitives;

namespace FleetManagement.Domain.Entities;

public class PaySchedule : Entity<Guid>
{
    public Guid TenantId { get; private set; }
    public PayScheduleFrequency Frequency { get; private set; }
    public DayOfWeek? DayOfWeek { get; private set; }
    public int? DayOfMonth { get; private set; }

    public PaySchedule(Guid id, Guid tenantId, PayScheduleFrequency frequency, DayOfWeek? dayOfWeek = null, int? dayOfMonth = null) : base(id)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        ValidateSchedule(frequency, dayOfWeek, dayOfMonth);

        TenantId = tenantId;
        Frequency = frequency;
        DayOfWeek = dayOfWeek;
        DayOfMonth = dayOfMonth;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    // Private constructor for EF Core
    private PaySchedule() : base(Guid.NewGuid())
    {
        TenantId = Guid.Empty;
    }

    public void Update(PayScheduleFrequency frequency, DayOfWeek? dayOfWeek = null, int? dayOfMonth = null)
    {
        ValidateSchedule(frequency, dayOfWeek, dayOfMonth);

        Frequency = frequency;
        DayOfWeek = dayOfWeek;
        DayOfMonth = dayOfMonth;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static void ValidateSchedule(PayScheduleFrequency frequency, DayOfWeek? dayOfWeek, int? dayOfMonth)
    {
        switch (frequency)
        {
            case PayScheduleFrequency.Weekly:
            case PayScheduleFrequency.BiWeekly:
                if (dayOfWeek == null)
                    throw new ArgumentException("DayOfWeek is required for weekly or bi-weekly frequency", nameof(dayOfWeek));
                break;
            case PayScheduleFrequency.Monthly:
                if (dayOfMonth == null || dayOfMonth < 1 || dayOfMonth > 31)
                    throw new ArgumentException("DayOfMonth must be between 1 and 31 for monthly frequency", nameof(dayOfMonth));
                break;
        }
    }
}
