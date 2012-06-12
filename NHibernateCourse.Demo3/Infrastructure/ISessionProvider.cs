using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;

namespace NHibernateCourse.Demo3.Infrastructure
{
    public interface ISessionProvider
    {
        ISession GetSession();
    }

    public class SessionProvider : ISessionProvider
    {
        private static ISessionFactory _sessionFactory;

        public ISession GetSession()
        {
            ISession session = null;
            if (_sessionFactory == null)
            {
                _sessionFactory = CreateSessionFactory();
                session = CreateSession();
            }

            if (session == null)
            {
                session = _sessionFactory.GetCurrentSession();

                if (!session.IsOpen || !session.IsConnected)
                    session = CreateSession();
            }

            session.Clear();

            return session;
        }

        private static ISession CreateSession()
        {
            var session = _sessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);

            return session;
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var cfg = new Configuration()
                .SetProperty(Environment.Hbm2ddlAuto, "create-drop")
                .SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote")
                .SetProperty(Environment.CurrentSessionContextClass, "thread_static")
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=NHibernateCourse;Integrated Security=SSPI";
                    db.Dialect<MsSql2008Dialect>();
                })
                .AddAssembly(Assembly.GetExecutingAssembly());

            return cfg.BuildSessionFactory();
        }
    }
}