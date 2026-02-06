using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Menu;
using Nop.Plugin.Misc.CustomerReminder.Services;


namespace Nop.Plugin.Misc.CustomerReminder.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerReminderService, CustomerReminderService>();
            services.AddScoped<IAdminMenu, CustomerReminderAdminMenu>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 1000;
    }
}
