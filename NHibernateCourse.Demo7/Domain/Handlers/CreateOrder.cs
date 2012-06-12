using System;
using NHibernate;
using NHibernateCourse.Demo7.Infrastructure;
using NHibernateCourse.Demo7.Infrastructure.Transactions;

namespace NHibernateCourse.Demo7.Domain.Handlers
{
    public interface ICreateOrder : ICommandHandler<CreateOrderCommand>
    {
    }

    public class CreateOrderCommand
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    [Transaction(true)]
    public class CreateOrder : ICreateOrder
    {
        private readonly ISession _session;

        public CreateOrder(ISession session)
        {
            _session = session;
        }

        public void Handle(CreateOrderCommand command)
        {
            var customer = _session.Get<Customer>(command.CustomerId);
            customer.MakeOrder(command.Description, command.Price);
        }
    }
}