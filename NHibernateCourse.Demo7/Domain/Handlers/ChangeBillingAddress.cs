using System;
using NHibernate;

namespace NHibernateCourse.Demo7.Domain.Handlers
{
    public class ChangeBillingAddressCommand
    {
        public Guid CustomerId { get; set; }
        public Address NewBillingAddress { get; set; }
    }

    public class ChangeBillingAddress
    {
        private readonly ISession _session;

        public ChangeBillingAddress(ISession session)
        {
            _session = session;
        }

        public void Handle(ChangeBillingAddressCommand command)
        {
            var customer = _session.Get<Customer>(command.CustomerId);
            customer.BillingAddress = command.NewBillingAddress;
        }
    }
}