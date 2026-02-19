using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CustomerReminder.Models;
public record CustomerReminderModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.Customer")]
    public int CustomerId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.ReminderTitle")]
    public string ReminderTitle { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.ReminderMessage")]
    public string ReminderMessage { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.ReminderDate")]
    public DateTime ReminderDate { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.IsSent")]
    public bool IsSent { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.CreatedOn")]
    public DateTime CreatedOnUtc { get; set; }

}
