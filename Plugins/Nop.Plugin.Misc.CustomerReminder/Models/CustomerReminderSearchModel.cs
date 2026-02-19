using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.CustomerReminder.Models;

public record CustomerReminderSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.Customer")]
    public int? CustomerId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.IsSent")]
    public bool? IsSent { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.FromDate")]
    public DateTime? FromDate { get; set; }

    [NopResourceDisplayName("Plugins.Misc.CustomerReminder.Fields.ToDate")]
    public DateTime? ToDate { get; set; }
}
