using System;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernateCourse.Demo3.Domain;
using NHibernateCourse.Demo3.Infrastructure;

namespace NHibernateCourse.Demo3
{
    class Program
    {
        static void Main()
        {
            NHibernateProfiler.Initialize();

            using (var session = new SessionProvider().GetSession())
            using (var transaction = session.BeginTransaction())
            {
                Console.WriteLine("Populating the DB");
                Console.WriteLine("--------------------");

                for (var i = 1; i <= 10; i++)
                {
                    session.Save(new Author
                    {
                        Name = "Author " + i
                    });
                }

                var random = new Random();
                for (var i = 1; i <= 10; i++)
                {
                    session.Save(new Book
                    {
                        Name = "Book " + i,
                        Price = random.Next(10, 50)
                    });
                }

                transaction.Commit();

                Console.WriteLine("Done");
                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}