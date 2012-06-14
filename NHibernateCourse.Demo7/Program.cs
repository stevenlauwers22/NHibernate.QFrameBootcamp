﻿using System;
using System.Collections.Generic;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernateCourse.Demo7.Domain;
using NHibernateCourse.Demo7.Domain.Handlers;
using NHibernateCourse.Demo7.Infrastructure;

namespace NHibernateCourse.Demo7
{
    class Program
    {
        static void Main()
        {
            PopulateDb();
            NHibernateProfiler.Initialize();
            
            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                Console.WriteLine("Get Belgian customers from the DB along with their total order price");
                Console.WriteLine("--------------------");

                var query = new GetCustomersFromBelgiumWithTotalOrderPrice(session);
                var customers = query.List().ToList();
                foreach (var customer in customers)
                {
                    Console.WriteLine("Customer: " + customer.Name + " total order price: " + customer.TotalOrderPrice);
                }

                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.WriteLine("Create a new order for the first Belgian customer");
                Console.WriteLine("--------------------");

                var makeOrder = new MakeOrder(session);
                var makeOrderCommand = new MakeOrderCommand
                {
                    CustomerId = customers.First().Id,
                    Description = Guid.NewGuid().ToString(),
                    Price = 20
                };

                makeOrder.Handle(makeOrderCommand);

                Console.WriteLine("Done");
                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.WriteLine("Change the billing address for the first Belgian customer");
                Console.WriteLine("--------------------");

                var changeBillingAddress = new ChangeBillingAddress(session);
                var changeBillingAddressCommand = new ChangeBillingAddressCommand
                {
                    CustomerId = customers.First().Id,
                    NewBillingAddress = new Address
                    {
                        Street = "Veldkant 33A",
                        City = "Kontich",
                        Country = "Belgie"
                    }
                };

                changeBillingAddress.Handle(changeBillingAddressCommand);

                Console.WriteLine("Done");
                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.ReadLine();

                transaction.Commit();
            }
        }

        public static void PopulateDb()
        {
            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                var countries = new List<string> { "Belgie", "Nederland", "Luxemburg" };
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
                        }
                    };

                    var numberOfOrdersToGenerate = random.Next(2, 10);
                    for (var o = 1; o < numberOfOrdersToGenerate; o++)
                    {
                        customer.MakeOrder(Guid.NewGuid().ToString(), random.Next(10, 50));
                    }

                    session.Save(customer);
                }

                transaction.Commit();
            }
        }
    }
}

