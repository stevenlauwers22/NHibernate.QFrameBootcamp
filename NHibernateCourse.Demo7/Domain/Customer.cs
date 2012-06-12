using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NHibernateCourse.Demo7.Infrastructure.Auditing;

namespace NHibernateCourse.Demo7.Domain
{
    public class Customer : IEntityWithAuditing
    {
        private readonly Guid _id;
        private readonly Iesi.Collections.Generic.ISet<Order> _orders;

        public Customer()
        {
            _id = Guid.Empty;
            _orders = new HashedSet<Order>();
        }

        public virtual Guid Id { get { return _id;  } }
        public virtual string Name { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual Address LegalAddress { get; set; }
        public virtual AuditingInfo AuditingInfo { get; set; }
        public virtual IEnumerable<Order> Orders { get { return _orders; } }

        public virtual void MakeOrder(string description, decimal price)
        {
            var order = new Order
            {
                Description = description,
                Price = price,
                Customer = this
            };

            _orders.Add(order);
        }
    }
}