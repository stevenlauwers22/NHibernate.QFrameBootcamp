using System;

namespace NHibernateCourse.Demo5.Domain
{
    public class Customer
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual Address LegalAddress { get; set; }
    }
}