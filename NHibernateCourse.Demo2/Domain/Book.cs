using System;
using Iesi.Collections.Generic;

namespace NHibernateCourse.Demo2.Domain
{
    public class Book
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual ISet<Author> Authors { get; set; }
    }
}