using FluentMigrator;
using Nop.Data.Migrations;

namespace Nop.Plugin.Misc.CustomerReminder.Data.Migrations
{
    [NopMigration("2026/02/06 12:00:00:0000000", "Misc.CustomerReminder: Add message template")]
    public class AddCustomerReminderMessageTemplate : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("MessageTemplate").Exists())
                return;

            Insert.IntoTable("MessageTemplate")
                .Row(new
                {
                    Name = "Customer.Reminder.Notification",
                    Subject = "Reminder: %CustomerReminder.Title%",
                    Body = @"
Hello %Customer.FullName%,<br/><br/>
You have a reminder scheduled on:
<b>%CustomerReminder.Date%</b> <br/><br/>
Message:<br/>
%CustomerReminder.Message%
<br/><br/>
Thank you,<br/>
%Store.Name%",
                    IsActive = true,
                    LimitedToStores = false,
                    EmailAccountId = 1,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    BccEmailAddresses = string.Empty,
                    AttachedDownloadId = 0,
                    AllowDirectReply = false
                });
        }

        public override void Down()
        {
            Delete.FromTable("MessageTemplate")
                .Row(new { Name = "Customer.Reminder.Notification" });
        }
    }
}
