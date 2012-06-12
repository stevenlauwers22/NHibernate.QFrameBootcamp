using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NHibernateCourse.Demo6.Infrastructure
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        void Save(TEntity tEntity);
        void Delete(TEntity tEntity);
    }

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ISessionProvider _sessionProvider;

        public Repository()
            : this(new SessionProvider())
        {
        }

        public Repository(ISessionProvider sessionProvider)
        {
            if (sessionProvider == null)
                throw new ArgumentNullException("sessionProvider");

            _sessionProvider = sessionProvider;
        }

        public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            var session = _sessionProvider.GetSession();
            return session.QueryOver<TEntity>().Where(predicate).List();
        }

        public IEnumerable<TEntity> GetAll()
        {
            var session = _sessionProvider.GetSession();
            return session.QueryOver<TEntity>().List();
        }

        public TEntity GetById(int id)
        {
            var session = _sessionProvider.GetSession();
            var result = session.Get<TEntity>(id);
            return result;
        }

        public void Save(TEntity tEntity)
        {
            var session = _sessionProvider.GetSession();
            session.SaveOrUpdate(tEntity);
            session.Flush();
        }

        public void Delete(TEntity tEntity)
        {
            var session = _sessionProvider.GetSession();
            session.Delete(tEntity);
        }
    }
}