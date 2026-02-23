using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.CustomerReminder.Factories;
using Nop.Plugin.Misc.CustomerReminder.Services;
using Nop.Web.Framework.Menu;


namespace Nop.Plugin.Misc.CustomerReminder.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerReminderService, CustomerReminderService>();
            services.AddScoped<ICustomerReminderModelFactory, CustomerReminderModelFactory>();
            services.AddScoped< CustomerReminderAdminMenuConsumer>();
            services.AddScoped<CustomerReminderEmailService>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 1000;
    }
}
