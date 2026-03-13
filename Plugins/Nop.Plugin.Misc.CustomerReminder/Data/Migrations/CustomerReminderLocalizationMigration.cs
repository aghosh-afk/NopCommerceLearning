using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.CustomerReminder.Data.Migrations;

[NopMigration("2026/03/05 12:00:00:0000000", "CustomerReminder localization", MigrationProcessType.Installation)]
public class CustomerReminderLocalizationMigration : Migration
{
    private readonly ILocalizationService _localizationService;

    public CustomerReminderLocalizationMigration(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public override void Up()
    {
        _localizationService.AddOrUpdateLocaleResource(new Dictionary<string, string>
        {
            // Page titles
            ["Plugins.Misc.CustomerReminder.Admin.Edit.Title"] = "Edit Customer Reminder",
            ["Plugins.Misc.CustomerReminder.Admin.Create.Title"] = "Create Customer Reminder",
            ["Plugins.Misc.CustomerReminder.Admin.List.Title"] = "Customer Reminders",

            // Buttons
            ["Plugins.Misc.CustomerReminder.Admin.Button.Save"] = "Save",
            ["Plugins.Misc.CustomerReminder.Admin.Button.Update"] = "Update",
            ["Plugins.Misc.CustomerReminder.Admin.Button.Cancel"] = "Cancel",
            ["Plugins.Misc.CustomerReminder.Admin.Button.ResetFilter"] = "Reset filter",
            ["Plugins.Misc.CustomerReminder.Admin.Button.Search"] = "Search",
            ["Plugins.Misc.CustomerReminder.Admin.Button.AddNew"] = "Search",
            ["Plugins.Misc.CustomerReminder.Admin.Button.AddNew"] = "Add New",

            // Dropdown / Select
            ["Plugins.Misc.CustomerReminder.Admin.SelectCustomer"] = "Select Customer...",

            // Fields
            ["Plugins.Misc.CustomerReminder.Fields.Customer"] = "Customer",
            ["Plugins.Misc.CustomerReminder.Fields.CustomerName"] = "Customer name",
            ["Plugins.Misc.CustomerReminder.Fields.ReminderTitle"] = "Reminder title",
            ["Plugins.Misc.CustomerReminder.Fields.ReminderMessage"] = "Reminder message",
            ["Plugins.Misc.CustomerReminder.Fields.ReminderDate"] = "Reminder date",
            ["Plugins.Misc.CustomerReminder.Fields.IsSent"] = "Is sent",
            ["Plugins.Misc.CustomerReminder.Fields.CreatedOn"] = "Created on",
            ["Plugins.Misc.CustomerReminder.Fields.FromDate"] = "From date",
            ["Plugins.Misc.CustomerReminder.Fields.ToDate"] = "To date",

            // Search / Filter
            ["Plugins.Misc.CustomerReminder.Admin.Search.Placeholder"] = "Search...",
            ["Plugins.Misc.CustomerReminder.Fields.Customer.Required"] = "Customer is required",
            ["Plugins.Misc.CustomerReminder.Fields.Title.Required"] = "Reminder title is required",
            ["Plugins.Misc.CustomerReminder.Fields.Message.Required"] = "Reminder message is required",
            ["Plugins.Misc.CustomerReminder.Fields.Date.Required"] = "Reminder date is required",
        });
    }

    public override void Down()
    {
        _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.CustomerReminder");
    }
}