namespace NHibernateCourse.Demo7.Infrastructure.Auditing
{
    public interface IEntityWithAuditing
    {
        AuditingInfo AuditingInfo { get; set; }
    }
}