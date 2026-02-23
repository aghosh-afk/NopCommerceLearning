using System.Threading.Tasks;
using Nop.Core.Domain.Menus;
using Nop.Web.Framework.Menu;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Microsoft.AspNetCore.Routing;

namespace Nop.Plugin.Misc.CustomerReminder.Infrastructure
{
    public class CustomerReminderAdminMenuConsumer : IConsumer<AdminMenuCreatedEvent>
    {
        public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
        {
            var root = eventMessage.RootMenuItem;

            var menuItem = new AdminMenuItem
            {
                SystemName = "CustomerReminder",
                Title = "Customer Reminder",
                Url= "/Admin/CustomerReminder/List",
                IconClass = "far fa-bell",
                Visible = true,
                
            };

            menuItem.ChildNodes.Add(new AdminMenuItem
            {
                SystemName = "CustomerReminder.List",
                Title = "All Reminder",
                Url = "/Admin/CustomerReminder/List",
                IconClass = "far fa-bell",
                Visible = true,
            });

            menuItem.ChildNodes.Add(new AdminMenuItem
            {
                SystemName = "CustomerReminder.Create",
                Title = "Create Reminder",
                Url = "/Admin/CustomerReminder/Create",
                IconClass = "far fa-bell",
                Visible = true,
            });

            //// ✅ Option 1: Add to root
            root.ChildNodes.Add(menuItem);

            // ✅ Option 2 (Recommended): Add under "Third party plugins"

            //var pluginNode = root.ChildNodes
            //    .FirstOrDefault(x => x.SystemName == "Third party plugins");

            //pluginNode?.ChildNodes.Add(menuItem);

            //await Task.CompletedTask;
        }
    }
}
