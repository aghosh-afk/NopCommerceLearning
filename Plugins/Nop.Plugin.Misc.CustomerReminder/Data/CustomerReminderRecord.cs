using System;
using LinqToDB.Mapping;
using Nop.Core;

namespace Nop.Plugin.Misc.CustomerReminder.Data;

[Table(Name = "CustomerReminderRecord")]   // MUST MATCH TABLE
public class CustomerReminderRecord : BaseEntity
{
    

    [Column, NotNull]
    public int CustomerId { get; set; }

    [Column, NotNull]
    public string ReminderTitle { get; set; } = string.Empty;

    [Column, NotNull]
    public string ReminderMessage { get; set; } = string.Empty;

    private DateTime _reminderDate;

    [Column, NotNull]
    public DateTime ReminderDate
    {
        get => _reminderDate;
        set => _reminderDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    [Column, NotNull]
    public bool IsSent { get; set; } = false;

    [Column, NotNull]
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
