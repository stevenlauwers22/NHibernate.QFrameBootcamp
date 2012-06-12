using System;
using NHibernate;
using NHibernateCourse.Demo7.Domain;
using NHibernateCourse.Demo7.Domain.Handlers;
using NHibernateCourse.Demo7.Infrastructure;
using NUnit.Framework;

namespace NHibernateCourse.Demo7.Tests
{
    [TestFixture]
    public class CatchNumberOfQueriesTest
    {
        [Test]
        public void CreateOrder_should_not_execute_more_than_one_query()
        {
            Customer customer;
            using (var session = new SessionProvider(IoC.GetContainer().Resolve<ISessionFactory>()).GetSession())
            using (var transaction = session.BeginTransaction())
            {
                customer = new Customer
                {
                    Name = Guid.NewGuid().ToString(),
                };
                session.Save(customer);
                transaction.Commit();
            }

            var command = IoC.GetContainer().Resolve<ICreateOrder>();
            using (3.Queries())
            {
                command.Handle(new CreateOrderCommand
                {
                    CustomerId = customer.Id,
                    Description = Guid.NewGuid().ToString(),
                    Price = 20
                });
            }
        }
    }
}
