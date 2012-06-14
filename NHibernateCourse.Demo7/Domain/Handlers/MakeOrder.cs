using System;
using NHibernate;

namespace NHibernateCourse.Demo7.Domain.Handlers
{
    public class MakeOrderCommand
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class MakeOrder
    {
        private readonly ISession _session;

        public MakeOrder(ISession session)
        {
            _session = session;
        }

        public void Handle(MakeOrderCommand command)
        {
            var customer = _session.Get<Customer>(command.CustomerId);
            customer.MakeOrder(command.Description, command.Price);
        }
    }
}