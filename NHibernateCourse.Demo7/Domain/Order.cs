using System;
using NHibernateCourse.Demo7.Infrastructure.Auditing;

namespace NHibernateCourse.Demo7.Domain
{
    public class Order : IEntityWithAuditing
    {
        private readonly Guid _id;

        public Order()
        {
            _id = Guid.Empty;
        }

        public virtual Guid Id { get { return _id; } }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual AuditingInfo AuditingInfo { get; set; }
        public virtual Customer Customer { get; set; }
    }
}