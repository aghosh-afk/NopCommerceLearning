using FluentMigrator;
using Nop.Data.Migrations;

namespace Nop.Plugin.Misc.CustomerReminder.Migrations
{
    [NopMigration("2026/02/06 15:30:00", "CustomerReminderRecord schema", MigrationProcessType.Installation)]
    public class CustomerReminderMigration : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Table("CustomerReminderRecord").Exists())
            {
                Create.Table("CustomerReminderRecord")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CustomerId").AsInt32().NotNullable()
                .WithColumn("ReminderTitle").AsString(255).NotNullable()
                .WithColumn("ReminderMessage").AsCustom("NVARCHAR(MAX)").NotNullable()
                .WithColumn("ReminderDate").AsCustom("datetime2").NotNullable()
                .WithColumn("IsSent").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedOnUtc")
                    .AsDateTime()
                    .NotNullable()
                    .WithDefault(SystemMethods.CurrentUTCDateTime);
            }
        }
    }
}
