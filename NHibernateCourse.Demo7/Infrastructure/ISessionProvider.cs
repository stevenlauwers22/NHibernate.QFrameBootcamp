using NHibernate;

namespace NHibernateCourse.Demo7.Infrastructure
{
    public interface ISessionProvider
    {
        ISession GetSession();
    }

    public class SessionProvider : ISessionProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;

        public SessionProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _session = CreateSession();
        }

        public ISession GetSession()
        {
            if (!_session.IsOpen || !_session.IsConnected)
                _session = CreateSession();

            return _session;
        }

        private ISession CreateSession()
        {
            var session = _sessionFactory.OpenSession();
            return session;
        }
    }
}