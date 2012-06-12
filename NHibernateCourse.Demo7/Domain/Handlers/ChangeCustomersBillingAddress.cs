using System;
using NHibernate;
using NHibernateCourse.Demo7.Infrastructure;
using NHibernateCourse.Demo7.Infrastructure.Transactions;

namespace NHibernateCourse.Demo7.Domain.Handlers
{
    public interface IChangeCustomerBillingAddress : ICommandHandler<ChangeCustomerBillingAddressCommand>
    {
    }

    public class ChangeCustomerBillingAddressCommand
    {
        public Guid CustomerId { get; set; }
        public Address BillingAddress { get; set; }
    }

    [Transaction(true)]
    public class ChangeCustomerBillingAddress : IChangeCustomerBillingAddress
    {
        private readonly ISession _session;

        public ChangeCustomerBillingAddress(ISession session)
        {
            _session = session;
        }

        public void Handle(ChangeCustomerBillingAddressCommand command)
        {
            var customer = _session.Get<Customer>(command.CustomerId);
            customer.BillingAddress = command.BillingAddress;
        }
    }
}