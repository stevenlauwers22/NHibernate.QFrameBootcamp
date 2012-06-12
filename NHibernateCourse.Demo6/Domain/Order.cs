using System;

namespace NHibernateCourse.Demo6.Domain
{
    public class Order
    {
        public virtual Guid Id { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual Customer Customer { get; set; }
    }
}