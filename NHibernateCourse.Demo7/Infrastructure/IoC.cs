using System.Reflection;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernateCourse.Demo7.Infrastructure.Transactions;

namespace NHibernateCourse.Demo7.Infrastructure
{
    public static class IoC
    {
        private static WindsorContainer _container;

        public static IWindsorContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new WindsorContainer();
                _container.AddFacility<TypedFactoryFacility>();
                _container
                    .Register(
                        Component
                            .For<ISessionFactory>()
                            .UsingFactoryMethod(fm => BuildSessionFactory())
                            .LifestyleSingleton())
                    .Register(
                        Component
                            .For<ISessionProvider>()
                            .UsingFactoryMethod(fm => new SessionProvider(fm.Resolve<ISessionFactory>()))
                            .LifestyleSingleton())
                    .Register(
                        Component
                            .For<ISession>()
                            .UsingFactoryMethod(fm => fm.Resolve<ISessionProvider>().GetSession())
                            .LifestyleTransient())
                    .Register(
                        Component
                            .For<TransactionInterceptor>()
                            .ImplementedBy<TransactionInterceptor>()
                            .LifestyleTransient())
                    .Register(
                        AllTypes
                            .FromThisAssembly()
                            .BasedOn<IQueryHandler>()
                            .WithService
                            .FromInterface(typeof (IQueryHandler))
                            .LifestyleTransient()
                            .Configure(c => c.Interceptors(InterceptorReference.ForType<TransactionInterceptor>()).AtIndex(0)))
                    .Register(
                        AllTypes
                            .FromThisAssembly()
                            .BasedOn<ICommandHandler>()
                            .WithService
                            .FromInterface(typeof (ICommandHandler))
                            .LifestyleTransient()
                            .Configure(c => c.Interceptors(InterceptorReference.ForType<TransactionInterceptor>()).AtIndex(0)))
                    ;
            }

            return _container;
        }

        private static ISessionFactory BuildSessionFactory()
        {
            var cfg = new Configuration()
                .SetProperty(Environment.Hbm2ddlAuto, "create-drop")
                .SetProperty(Environment.Hbm2ddlKeyWords, "auto-quote")
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=NHibernateCourse;Integrated Security=SSPI";
                    db.Dialect<MsSql2008Dialect>();
                })
                .AddAssembly(Assembly.GetExecutingAssembly());

            //cfg.SetListener(ListenerType.FlushEntity, new AuditingFlushEntityEventListener());
            //cfg.SetListener(ListenerType.PreInsert, new AuditingEventListener());
            //cfg.SetListener(ListenerType.PreUpdate, new AuditingEventListener());
            //cfg.SetInterceptor(new DontHurtMeInterceptor());

            return cfg.BuildSessionFactory();
        }
    }
}
