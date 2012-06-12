using System;
using Iesi.Collections.Generic;

namespace NHibernateCourse.Demo6.Domain
{
    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual Address LegalAddress { get; set; }
        public virtual ISet<Order> Orders { get; set; }
    }
}