using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;

namespace NHibernateCourse.Demo7.Infrastructure.Auditing
{
    public class AuditingFlushEntityEventListener : DefaultFlushEntityEventListener
    {
        protected override void DirtyCheck(FlushEntityEvent @event)
        {
            base.DirtyCheck(@event);
            if (@event.DirtyProperties == null || !@event.DirtyProperties.Any())
                return;

            var auditableEntity = @event.Entity as IEntityWithAuditing;
            if (auditableEntity == null)
                return;

            @event.DirtyProperties = @event
                .DirtyProperties
                .Concat(GetAdditionalDirtyProperties(@event.EntityEntry.Persister))
                .ToArray();
        }

        private static IEnumerable<int> GetAdditionalDirtyProperties(IEntityPersister persister)
        {
            // Always update audit fields, even if their values don't change (dynamic-update=true would prevent this, hence this 'hack')
            var auditingInfoPropertyIndex = Array.IndexOf(persister.PropertyNames, "AuditingInfo");
            if (auditingInfoPropertyIndex != -1)
                yield return auditingInfoPropertyIndex;
        }
    }
}