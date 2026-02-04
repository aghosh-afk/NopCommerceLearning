using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Web.Infrastructure
{
    public class AdminMenuConsumer : IConsumer<AdminMenuCreatedEvent>
    {
        public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
        {
            eventMessage.RootMenuItem.InsertAfter("Configuration",
                new AdminMenuItem
                {
                    SystemName = "SimpleNotes",
                    Title = "Simple Notes",
                    Url = eventMessage.GetMenuItemUrl("SimpleNote", "Index"),
                    IconClass = "fa fa-sticky-note",
                    Visible = true
                });

            return Task.CompletedTask;
        }
    }
}
