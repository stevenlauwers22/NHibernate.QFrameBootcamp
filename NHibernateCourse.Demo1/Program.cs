using System;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernateCourse.Demo1.Domain;
using NHibernateCourse.Demo1.Infrastructure;

namespace NHibernateCourse.Demo1
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
                Console.WriteLine("Get all books from the DB");
                Console.WriteLine("--------------------");

                // Query with lazy load
                var books = session
                    .QueryOver<Book>()
                    .OrderBy(b => b.Name).Asc
                    .List();

                // Query with eager load
                //var books = session
                //    .QueryOver<Book>()
                //    .OrderBy(b => b.Name).Asc
                //    .Left.JoinQueryOver(b => b.Author)
                //    .List();

                foreach (var book in books)
                {
                    Console.WriteLine("Book: " + book.Name + " written by " + book.Author.Name);
                }

                Console.WriteLine("--------------------");
                Console.WriteLine();
                Console.ReadLine();

                transaction.Rollback();
            }
        }

        private static void PopulateDb()
        {
            using (var session = new SessionProvider().GetSession())
            using (var transation = session.BeginTransaction())
            {
                for (var i = 1; i <= 50; i++)
                {
                    session.Save(new Author
                    {
                        Name = "Author " + i
                    });
                }

                var authors = session.QueryOver<Author>().List();
                var random = new Random();
                for (var i = 1; i <= 100; i++)
                {
                    session.Save(new Book
                    {
                        Name = "Book " + i,
                        Price = random.Next(10, 50),
                        Author = authors.ElementAt(random.Next(0, authors.Count()))
                    });
                }

                transation.Commit();
            }
        }
    }
}