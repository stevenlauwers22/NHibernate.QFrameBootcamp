using System;
using System.Collections.Generic;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace NHibernateCourse.Demo7.Infrastructure.Auditing
{
    public class AuditingEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            var auditableEntity = @event.Entity as IEntityWithAuditing;
            if (auditableEntity == null)
                return false;

            auditableEntity.AuditingInfo = new AuditingInfo
            {
                CreatedBy = "Me",
                CreatedOn = DateTime.Now
            };
            Set(@event.Persister, @event.State, "AuditingInfo", auditableEntity.AuditingInfo);

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var auditableEntity = @event.Entity as IEntityWithAuditing;
            if (auditableEntity == null)
                return false;
            
            auditableEntity.AuditingInfo.UpdatedBy = "Someone else";
            auditableEntity.AuditingInfo.UpdatedOn = DateTime.Now;
            Set(@event.Persister, @event.State, "AuditingInfo", auditableEntity.AuditingInfo);

            return false;
        }

        private static void Set(IEntityPersister persister, IList<object> state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;

            state[index] = value;
        }
    }
}