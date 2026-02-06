using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Services.Customers;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public class CustomerReminderService : ICustomerReminderService
    {
        private readonly IRepository<CustomerReminderRecord> _repository;
        private readonly ICustomerService _customerService;
        private readonly CustomerReminderEmailService _emailService;

        public CustomerReminderService(
            IRepository<CustomerReminderRecord> repository,
            ICustomerService customerService,
            CustomerReminderEmailService emailService)
        {
            _repository = repository;
            _customerService = customerService;
            _emailService = emailService;
        }

        public async Task<IList<CustomerReminderRecord>> GetAllAsync()
        {
            return await _repository.Table.OrderByDescending(x => x.CreatedOnUtc).ToListAsync();
        }

        public async Task<CustomerReminderRecord?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task InsertAsync(CustomerReminderRecord record)
        {
            // Step 1: Insert the reminder into the database
            await _repository.InsertAsync(record);

            // Step 2: Get the Customer entity from the database
            var customer = await _customerService.GetCustomerByIdAsync(record.CustomerId);

            // Step 3: Send reminder email using our Email Service
            if (customer != null)
                _emailService.SendReminder(record, customer);
        }

        public async Task UpdateAsync(CustomerReminderRecord record)
        {
            await _repository.UpdateAsync(record);
        }

        public async Task DeleteAsync(CustomerReminderRecord record)
        {
            await _repository.DeleteAsync(record);
        }
    }
}
