using System;
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
            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                customer = new Customer
                {
                    Name = Guid.NewGuid().ToString(),
                };
                session.Save(customer);
                transaction.Commit();
            }

            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                var makeOrder = new MakeOrder(session);
                var makeOrderCommand = new MakeOrderCommand
                {
                    CustomerId = customer.Id,
                    Description = Guid.NewGuid().ToString(),
                    Price = 20
                };

                using (3.Queries())
                {
                    makeOrder.Handle(makeOrderCommand);
                    transaction.Commit();
                }
            }
        }
    }
}
