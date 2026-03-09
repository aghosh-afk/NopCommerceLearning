using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Misc.CustomerReminder.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.CustomerReminder.Validators;

public class CustomerReminderValidator : BaseNopValidator<CustomerReminderModel>
{
    public CustomerReminderValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.CustomerId)
                 .GreaterThan(0)
                 .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CustomerReminder.Fields.Customer.Required"));

        RuleFor(x => x.ReminderTitle)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CustomerReminder.Fields.Title.Required"));

        RuleFor(x => x.ReminderMessage)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CustomerReminder.Fields.Message.Required"));

        RuleFor(x => x.ReminderDate)
            .NotNull()
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Misc.CustomerReminder.Fields.Date.Required"));
    }
}
