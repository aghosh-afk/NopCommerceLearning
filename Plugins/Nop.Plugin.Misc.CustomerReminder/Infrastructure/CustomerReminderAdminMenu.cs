using System.Threading.Tasks;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.CustomerReminder.Infrastructure
{
    public class CustomerReminderAdminMenu : IAdminMenu
    {
        // REQUIRED by interface (nopCommerce 4.90.x)
        public string GetMenuItemUrl(string controllerName, string actionName)
        {
            return $"/Admin/{controllerName}/{actionName}";
        }

        public async Task<AdminMenuItem> GetRootNodeAsync(bool showHidden = false)
        {
            var rootNode = new AdminMenuItem
            {
                SystemName = "CustomerReminder",
                Title = "Customer Reminder",
                Visible = true,
                Url = "/Admin/CustomerReminder/List",
                ChildNodes =
                {
                    new AdminMenuItem
                    {
                        SystemName = "CustomerReminder.List",
                        Title = "All Reminders",
                        Visible = true,
                        Url = "/Admin/CustomerReminder/List"
                    },
                    new AdminMenuItem
                    {
                        SystemName = "CustomerReminder.Create",
                        Title = "Add Reminder",
                        Visible = true,
                        Url = "/Admin/CustomerReminder/Create"
                    }
                }
            };

            return await Task.FromResult(rootNode);
        }
    }
}
