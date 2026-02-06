using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Common;
using Nop.Services.Messages;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.CustomerReminder
{
    public class CustomerReminderPlugin : BasePlugin, IMiscPlugin
    {
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IWebHelper _webHelper;

        public CustomerReminderPlugin(
            IMessageTemplateService messageTemplateService,
            IWebHelper webHelper)
        {
            _messageTemplateService = messageTemplateService;
            _webHelper = webHelper;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/CustomerReminder/List";
        }

        public override async Task InstallAsync()
        {
            // ✅ DO NOT RUN MIGRATION HERE — nopCommerce runs it automatically

            var templates = await _messageTemplateService.GetAllMessageTemplatesAsync(0);

            if (!templates.Any(t => t.Name == "Customer.Reminder"))
            {
                var template = new MessageTemplate
                {
                    Name = "Customer.Reminder",
                    Subject = "Reminder: %Reminder.Title%",
                    Body = "Hello %Customer.Name%,<br/>%Reminder.Message%",
                    IsActive = true,
                    EmailAccountId = 0
                };

                await _messageTemplateService.InsertMessageTemplateAsync(template);
            }

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            var templates = await _messageTemplateService.GetAllMessageTemplatesAsync(0);
            var template = templates.FirstOrDefault(t => t.Name == "Customer.Reminder");

            if (template != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(template);

            await base.UninstallAsync();
        }
    }
}
