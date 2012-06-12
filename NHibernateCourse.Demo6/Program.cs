using System;
using System.Collections.Generic;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Iesi.Collections.Generic;
using NHibernateCourse.Demo6.Domain;
using NHibernateCourse.Demo6.Infrastructure;

namespace NHibernateCourse.Demo6
{
    class Program
    {
        static void Main()
        {
            PopulateDb();
            NHibernateProfiler.Initialize();

            Console.WriteLine("Get Belgian customers from the DB along with their total order price");
            Console.WriteLine("--------------------");

            IRepository<Customer> customerRepository = new Repository<Customer>();
            var customers = customerRepository.Query(c => c.BillingAddress.Country == "Belgie").ToList();
            foreach (var customer in customers)
            {
                Console.WriteLine("Customer: " + customer.Name + " total order price: " + customer.Orders.Sum(o => o.Price));
            }

            Console.WriteLine("--------------------");
            Console.WriteLine();
            Console.WriteLine("Create a new order for the first Belgian customer");
            Console.WriteLine("--------------------");

            var customer1 = customers.First();
            var order = new Order
            {
                Description = "A new order",
                Price = 20,
                Customer = customer1
            };

            customer1.Orders.Add(order);
            customerRepository.Save(customer1);

            Console.WriteLine("Done");
            Console.WriteLine("--------------------");
            Console.WriteLine();
            Console.ReadLine();
        }

        private static void PopulateDb()
        {
            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                var countries = new List<string> {"Belgie", "Nederland", "Luxemburg"};
                var random = new Random();
                for (var i = 1; i <= 10; i++)
                {
                    var customer = new Customer
                    {
                        Name = "Customer " + i,
                        BillingAddress = new Address
                        {
                            Street = "Billing Street " + i,
                            City = "Billing City " + i,
                            Country = countries.ElementAt(random.Next(0, countries.Count - 1))
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
                        },
                        Orders = new HashedSet<Order>()
                    };

                    var numberOfOrdersToGenerate = random.Next(2, 10);
                    for (var o = 1; o < numberOfOrdersToGenerate; o++)
                    {
                        var order = new Order
                        {
                            Description = Guid.NewGuid().ToString(),
                            Price = random.Next(10, 50),
                            Customer = customer
                        };

                        customer.Orders.Add(order);
                    }

                    session.Save(customer);
                }

                transaction.Commit();
            }
        }
    }
}