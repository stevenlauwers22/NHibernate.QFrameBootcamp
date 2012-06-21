using System;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Iesi.Collections.Generic;
using NHibernateCourse.Demo2.Domain;
using NHibernateCourse.Demo2.Infrastructure;

namespace NHibernateCourse.Demo2
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

                // Query with duplicate data
                var books = session
                    .QueryOver<Book>()
                    .OrderBy(b => b.Name).Asc
                    .Left.JoinQueryOver(b => b.Authors)
                    .List();

                // Query with duplicate data fixed by distinct root entity transformer
                //var books = session
                //    .QueryOver<Book>()
                //    .OrderBy(b => b.Name).Asc
                //    .Left.JoinQueryOver(b => b.Authors)
                //    .TransformUsing(Transformers.DistinctRootEntity)
                //    .List();

                // Query with duplicate data fixed by future query
                //var books = session
                //    .QueryOver<Book>()
                //    .OrderBy(b => b.Name).Asc
                //    .Future<Book>();

                //session
                //    .QueryOver<Book>()
                //    .Left.JoinQueryOver(b => b.Authors)
                //    .Future<Book>();

                foreach (var book in books)
                {
                    Console.WriteLine("Book: " + book.Name + " written by " + book.Authors.Select(a => a.Name).Aggregate((a1, a2) => a1 + ", " + a2));
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
            using (var transaction = session.BeginTransaction())
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
                    var book = new Book
                    {
                        Name = "Book " + i,
                        Price = random.Next(10, 50)
                    };

                    var randomAuthor = random.Next(0, authors.Count() - 1);
                    book.Authors = new HashedSet<Author>
                    {
                        authors.ElementAt(randomAuthor),
                        authors.ElementAt(randomAuthor + 1)
                    };
                    session.Save(book);
                }

                transaction.Commit();
            }
        }
    }
}