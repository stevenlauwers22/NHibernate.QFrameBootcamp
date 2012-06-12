using System;

namespace NHibernateCourse.Demo7.Infrastructure.Auditing
{
    public class AuditingInfo
    {
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedOn { get; set; }
    }
}