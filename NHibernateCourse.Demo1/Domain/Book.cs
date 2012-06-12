using System;

namespace NHibernateCourse.Demo1.Domain
{
    public class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual Author Author { get; set; }
    }
}