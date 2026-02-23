using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Services.Customers;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public class CustomerReminderService : ICustomerReminderService
    {
        private readonly IRepository<CustomerReminderRecord> _repository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly CustomerReminderEmailService _emailService;

        public CustomerReminderService(
            IRepository<CustomerReminderRecord> repository,
            ICustomerService customerService,
            CustomerReminderEmailService emailService,
            IRepository<Customer> customerRepository)
        {
            _repository = repository;
            _customerService = customerService;
            _emailService = emailService;
            _customerRepository = customerRepository;
        }

        public async Task<IPagedList<CustomerReminderRecord>> GetAllPagedAsync(
    int pageIndex,
    int pageSize,
    int? customerId = null,
    bool? isSent = null,
    DateTime? fromDate = null,
    DateTime? toDate = null)
        {
            var query = _repository.Table;

            if (customerId.HasValue)
                query = query.Where(x => x.CustomerId == customerId.Value);

            if (isSent.HasValue)
                query = query.Where(x => x.IsSent == isSent.Value);

            if (fromDate.HasValue)
                query = query.Where(x => x.ReminderDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.ReminderDate <= toDate.Value);

            return await query.ToPagedListAsync(pageIndex, pageSize);
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
        public async Task<(IList<Customer> Customers, int TotalCount)> SearchCustomersAsync(string search, int page, int pageSize)
        {
            var query = _customerRepository.Table;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(c =>
                    c.FirstName.Contains(search) ||
                    c.LastName.Contains(search));
            }

            var total = await query.CountAsync();

            var customers = await query
                .OrderBy(c => c.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (customers, total);
        }
    }
}
