using System;
using ServiceHub.Domain.Common.Base;

namespace ServiceHub.Domain.Identity.Entities;

/// <summary>
/// Represents weekly working schedule windows for local/offline vendor services.
/// </summary>
public class VendorAvailability : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the linked vendor profile.
    /// </summary>
    public Guid VendorProfileId { get; private set; }

    /// <summary>
    /// Gets the day of the week.
    /// </summary>
    public DayOfWeek DayOfWeek { get; private set; }

    /// <summary>
    /// Gets the start time of the availability window.
    /// </summary>
    public TimeSpan StartTime { get; private set; }

    /// <summary>
    /// Gets the end time of the availability window.
    /// </summary>
    public TimeSpan EndTime { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the vendor is available during this time slot.
    /// </summary>
    public bool IsAvailable { get; private set; }

    // Navigation property

    /// <summary>
    /// Gets the linked vendor profile.
    /// </summary>
    public virtual VendorProfile VendorProfile { get; private set; } = null!;

    // Parameterless constructor for EF Core materialization
    private VendorAvailability()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorAvailability"/> class.
    /// </summary>
    public VendorAvailability(Guid vendorProfileId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime, bool isAvailable = true)
    {
        if (vendorProfileId == Guid.Empty)
            throw new ArgumentException("Vendor profile ID cannot be empty.", nameof(vendorProfileId));

        VendorProfileId = vendorProfileId;
        DayOfWeek = dayOfWeek;
        IsAvailable = isAvailable;
        UpdateHours(startTime, endTime);
    }

    /// <summary>
    /// Updates the operating time window hours.
    /// </summary>
    public void UpdateHours(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("End time must be greater than start time.");

        StartTime = startTime;
        EndTime = endTime;
    }
}
