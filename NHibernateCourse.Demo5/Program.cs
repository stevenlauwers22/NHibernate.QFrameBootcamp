using System;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernateCourse.Demo5.Domain;
using NHibernateCourse.Demo5.Infrastructure;

namespace NHibernateCourse.Demo5
{
    class Program
    {
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            PopulateDb();
            NHibernateProfiler.Initialize();

            Console.WriteLine("Get a customer from the DB");
            Console.WriteLine("--------------------");

            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                var customer = session
                    .QueryOver<Customer>()
                    .Where(c => c.Name == "Customer 1")
                    .SingleOrDefault();

                Console.WriteLine("Customer: " + customer.Name);
                Console.WriteLine("Billing address: {0} {1} {2}", customer.BillingAddress.Street, customer.BillingAddress.City, customer.BillingAddress.Country);
                Console.WriteLine("Shipping address: {0} {1} {2}", customer.ShippingAddress.Street, customer.ShippingAddress.City, customer.ShippingAddress.Country);
                Console.WriteLine("Legal address: {0} {1} {2}", customer.LegalAddress.Street, customer.LegalAddress.City, customer.LegalAddress.Country);
                Console.WriteLine("--------------------");
                Console.WriteLine();

                Console.WriteLine("Press any key to update the customer");
                Console.WriteLine("--------------------");
                Console.ReadLine();

                customer.LegalAddress.Street = "Veldkant 33A";
                customer.LegalAddress.City = "Kontich";
                customer.LegalAddress.Country = "Belgie";
                transaction.Commit();

                Console.WriteLine("Done");
                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.ReadLine();
            }
        }

        private static void PopulateDb()
        {
            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                for (var i = 1; i <= 10; i++)
                {
                    session.Save(new Customer
                    {
                        Name = "Customer " + i,
                        BillingAddress = new Address
                        {
                            Street = "Billing Street " + i,
                            City = "Billing City " + i,
                            Country = "Billing Country " + i
                        },
                        ShippingAddress = new Address
                        {
                            Street = "Shipping Street " + i,
                            City = "Shipping City " + i,
                            Country = "Shipping Country " + i
                        },
                        LegalAddress = new Address
                        {
                            Street = "Legal Street " + i,
                            City = "Legal City " + i,
                            Country = "Legal Country " + i
                        }
                    });
                }

                transaction.Commit();
            }
        }
    }
}

