using System;
using System.Linq;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Plugin.Misc.CustomerReminder.Data;
using Nop.Services.Messages;
using Nop.Services.Stores;

namespace Nop.Plugin.Misc.CustomerReminder.Services
{
    public class CustomerReminderEmailService
    {
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IRepository<QueuedEmail> _queuedEmailRepository;
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IStoreService _storeService;

        public CustomerReminderEmailService(
            IRepository<MessageTemplate> messageTemplateRepository,
            IRepository<QueuedEmail> queuedEmailRepository,
            IRepository<EmailAccount> emailAccountRepository,
            IStoreService storeService)
        {
            _messageTemplateRepository = messageTemplateRepository;
            _queuedEmailRepository = queuedEmailRepository;
            _emailAccountRepository = emailAccountRepository;
            _storeService = storeService;
        }

        public void SendReminder(CustomerReminderRecord reminder, Customer customer)
        {
            var template = _messageTemplateRepository.Table
                .FirstOrDefault(t => t.Name == "Customer.Reminder.Notification" && t.IsActive);

            if (template == null)
                return;

            var store = _storeService.GetAllStores().FirstOrDefault();
            var storeName = store?.Name ?? "Store";

            var emailAccount = _emailAccountRepository.Table
                .FirstOrDefault(e => e.Id == template.EmailAccountId)
                ?? _emailAccountRepository.Table.FirstOrDefault();

            var fromEmail = emailAccount?.Email ?? "admin@store.com";
            var fromName = emailAccount?.DisplayName ?? storeName;

            var customerFullName = $"{customer.FirstName} {customer.LastName}";

            var body = template.Body
                .Replace("%Customer.FullName%", customerFullName)
                .Replace("%Store.Name%", storeName)
                .Replace("%CustomerReminder.Title%", reminder.ReminderTitle)
                .Replace("%CustomerReminder.Message%", reminder.ReminderMessage)
                .Replace("%CustomerReminder.Date%", reminder.ReminderDate.ToString("yyyy-MM-dd HH:mm"));

            var subject = template.Subject
                .Replace("%CustomerReminder.Title%", reminder.ReminderTitle);

            var queuedEmail = new QueuedEmail
            {
                From = fromEmail,
                FromName = fromName,
                To = customer.Email,
                ToName = customerFullName,
                Subject = subject,
                Body = body,
                CreatedOnUtc = DateTime.UtcNow,
                DontSendBeforeDateUtc = null,   // <-- allow immediate sending
                SentOnUtc = null,               // <-- must be null so task can send it
                EmailAccountId = emailAccount?.Id ?? 0,
                Priority = QueuedEmailPriority.High,
            };

            _queuedEmailRepository.Insert(queuedEmail);
        }
    }
}
